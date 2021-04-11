using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class MoveComponentDestroySystem: DestroySystem<FrameMoveComponent>
    {
        public override void Destroy(FrameMoveComponent self)
        {
            self.Clear();
        }
    }

    [ObjectSystem]
    public class MoveComponentAwakeSystem: AwakeSystem<FrameMoveComponent>
    {
        public override void Awake(FrameMoveComponent self)
        {
            self.StartTime = 0;
            self.StartPos = Vector3.zero;
            self.NeedTime = 0;
            self.MoveTimer = 0;
            self.Callback = null;
            self.Targets.Clear();
            self.Speed = 0;
            self.N = 0;
            self.TurnTime = 0;
        }
    }

    public static class MoveComponentSystem
    {
        public static bool IsArrived(this FrameMoveComponent self)
        {
            return self.Targets.Count == 0;
        }

        public static bool ChangeSpeed(this FrameMoveComponent self, float speed)
        {
            if (self.IsArrived())
            {
                return false;
            }

            if (speed < 0.0001)
            {
                return false;
            }
            
            Unit unit = self.GetParent<Unit>();
            using (ListComponent<Vector3> path = ListComponent<Vector3>.Create())
            {
                self.MoveForward(true);
                
                path.List.Add(unit.Position); // 第一个是Unit的pos
                for (int i = self.N; i < self.Targets.Count; ++i)
                {
                    path.List.Add(self.Targets[i]);
                }
                self.MoveToAsync(path.List, speed).Coroutine();
            }

            return true;
        }

        public static async ETTask<bool> MoveToAsync(this FrameMoveComponent self, List<Vector3> target, float speed, bool isSimulate =false,int frameTime = 0, int turnTime = 100, ETCancellationToken cancellationToken = null)
        {
            self.Stop();

            foreach (Vector3 v in target)
            {
                self.Targets.Add(v);
            }

            self.IsTurnHorizontal = true;
            self.TurnTime = turnTime;
            self.Speed = speed;
            if (isSimulate)
            {
                self.StartMove(isSimulate,frameTime);
                return true;
            }

            ETTaskCompletionSource<bool> tcs = new ETTaskCompletionSource<bool>();
            self.Callback = (ret) => { tcs.SetResult(ret); };

            Game.EventSystem.Publish(new EventIDType.MoveStart(){Unit = self.GetParent<Unit>()}).Coroutine();
            
            self.StartMove();
            
            void CancelAction()
            {
                self.Stop();
            }
            
            bool moveRet;
            try
            {
                cancellationToken?.Add(CancelAction);
                moveRet = await tcs.Task;
            }
            finally
            {
                cancellationToken?.Remove(CancelAction);
            }

            if (moveRet)
            {
                Game.EventSystem.Publish(new EventIDType.MoveStop(){Unit = self.GetParent<Unit>()}).Coroutine();
            }
            return moveRet;
        }

        public static void MoveForward(this FrameMoveComponent self, bool needCancel,long frameTime = 0)
        {
            Unit unit = self.GetParent<Unit>();

            if (frameTime == 0)
                frameTime = self.GetCurrSimulateFrame();
            long timeNow = frameTime;
            long moveTime = timeNow - self.StartTime;

            while (true)
            {
                if (moveTime <= 0)
                {
                    return;
                }
                
                // 计算位置插值
                if (moveTime >= self.NeedTime)
                {
                    unit.Position = self.NextTarget;
                    if (self.TurnTime > 0)
                    {
                        unit.Rotation = self.To;
                    }
                }
                else
                {
                    // 计算位置插值
                    float amount = moveTime * 1f / self.NeedTime;
                    if (amount > 0)
                    {
                        Vector3 newPos = Vector3.Lerp(self.StartPos, self.NextTarget, amount);
                        unit.Position = newPos;
                    }
                    
                    // 计算方向插值
                    if (self.TurnTime > 0)
                    {
                        amount = moveTime * 1f / self.TurnTime;
                        Quaternion q = Quaternion.Slerp(self.From, self.To, amount);
                        unit.Rotation = q;
                    }
                }

                moveTime -= self.NeedTime;

                // 表示这个点还没走完，等下一帧再来
                if (moveTime < 0)
                {
                    return;
                }
                
                // 到这里说明这个点已经走完
                
                // 如果是最后一个点
                if (self.N >= self.Targets.Count - 1)
                {
                    unit.Position = self.NextTarget;
                    unit.Rotation = self.To;

                    Action<bool> callback = self.Callback;
                    self.Callback = null;

                    self.Clear();
                    callback?.Invoke(!needCancel);
                    return;
                }

                self.SetNextTarget();
            }
        }

        private static void StartMove(this FrameMoveComponent self,bool simulate = false,long startTime = 0)
        {
            Unit unit = self.GetParent<Unit>();
            if (startTime == 0)
                startTime = self.GetCurrSimulateFrame();
            self.BeginTime = startTime;
            self.StartTime = self.BeginTime;
            self.SetNextTarget();
            if (simulate) return;
            self.MoveTimer = self.Domain.GetComponent<FrameTimerComponent>().NewRepeatedFrameTimer(1,() =>
                    {
                        try
                        {
                            self.MoveForward(false);
                        }
                        catch (Exception e)
                        {
                            Log.Error($"move timer error: {unit.Id}\n{e}");
                        }
                    }
            );
        }

        private static void SetNextTarget(this FrameMoveComponent self)
        {

            Unit unit = self.GetParent<Unit>();

            ++self.N;

            // 时间计算用服务端的位置, 但是移动要用客户端的位置来插值
            Vector3 v = self.GetFaceV();
            float distance = v.magnitude;
            
            // 插值的起始点要以unit的真实位置来算
            self.StartPos = unit.Position;

            self.StartTime += self.NeedTime;
            
            self.NeedTime = (long) (distance / self.Speed * 1000) / Game.ClientFrameDuration;

            
            if (self.TurnTime > 0)
            {
                // 要用unit的位置
                Vector3 faceV = self.GetFaceV();
                if (faceV.sqrMagnitude < 0.0001f)
                {
                    return;
                }
                self.From = unit.Rotation;
                
                if (self.IsTurnHorizontal)
                {
                    faceV.y = 0;
                }

                if (Math.Abs(faceV.x) > 0.01 || Math.Abs(faceV.z) > 0.01)
                {
                    self.To = Quaternion.LookRotation(faceV, Vector3.up);
                }

                return;
            }
            
            if (self.TurnTime == 0) // turn time == 0 立即转向
            {
                Vector3 faceV = self.GetFaceV();
                if (self.IsTurnHorizontal)
                {
                    faceV.y = 0;
                }

                if (Math.Abs(faceV.x) > 0.01 || Math.Abs(faceV.z) > 0.01)
                {
                    self.To = Quaternion.LookRotation(faceV, Vector3.up);
                    unit.Rotation = self.To;
                }
            }
        }

        private static Vector3 GetFaceV(this FrameMoveComponent self)
        {
            return self.NextTarget - self.PreTarget;
        }

        public static bool FlashTo(this FrameMoveComponent self, Vector3 target)
        {
            Unit unit = self.GetParent<Unit>();
            unit.Position = target;
            return true;
        }

        public static bool MoveTo(this FrameMoveComponent self, Vector3 target, float speed, int turnTime = 0, bool isTurnHorizontal = false)
        {
            if (speed < 0.001)
            {
                Log.Error($"speed is 0 {self.GetParent<Unit>().Config.Id} {self.GetParent<Unit>().Id} {speed}");
                return false;
            }

            self.Stop();

            self.IsTurnHorizontal = isTurnHorizontal;
            self.TurnTime = turnTime;
            self.Targets.Add(self.GetParent<Unit>().Position);
            self.Targets.Add(target);
            self.Speed = speed;

            self.StartMove();
            return true;
        }

        public static bool MoveTo(this FrameMoveComponent self, List<Vector3> target, float speed, int turnTime = 0)
        {
            if (target.Count == 0)
            {
                return true;
            }

            if (Math.Abs(speed) < 0.001)
            {
                Log.Error($"speed is 0 {self.GetParent<Unit>().Config.Id} {self.GetParent<Unit>().Id}");
                return false;
            }

            self.Stop();

            foreach (Vector3 v in target)
            {
                self.Targets.Add(v);
            }

            self.IsTurnHorizontal = true;
            self.TurnTime = turnTime;
            self.Speed = speed;

            self.StartMove();

            return true;
        }

        public static void Stop(this FrameMoveComponent self)
        {
            if (self.Targets.Count > 0)
            {
                self.MoveForward(true);
            }

            self.Clear();
        }

        public static void Clear(this FrameMoveComponent self)
        {
            self.StartTime = 0;
            self.StartPos = Vector3.zero;
            self.BeginTime = 0;
            self.NeedTime = 0;
            TimerComponent.Instance.Remove(ref self.MoveTimer);
            self.Targets.Clear();
            self.Speed = 0;
            self.N = 0;
            self.TurnTime = 0;
            self.IsTurnHorizontal = false;

            if (self.Callback != null)
            {
                Action<bool> callback = self.Callback;
                self.Callback = null;
                callback.Invoke(false);
            }
        }
    }
}