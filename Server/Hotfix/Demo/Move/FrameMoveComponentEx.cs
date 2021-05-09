
using System.Collections.Generic;
using System.IO;
using ET;
using JetBrains.Annotations;
using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class FrameMoveComponentAwakeSystem: AwakeSystem<FrameMoveComponent>
    {
        public override void Awake(FrameMoveComponent self)
        {
            self.moveTimer = self.Domain.GetComponent<FrameTimerComponent>().NewRepeatedFrameTimer(1, self.RunNext);
        }
    }
    
    [ObjectSystem]
    public class FrameMoveComponentDestroySystem: DestroySystem<FrameMoveComponent>
    {
        public override void Destroy(FrameMoveComponent self)
        {
            self.Domain.GetComponent<FrameTimerComponent>().Remove(ref self.moveTimer);
        }
    }


    public static class FrameMoveComponentSystemEx
    {
        public static void Run(this FrameMoveComponent self)
        {
            if (self.CurrPathIndex == 0 
                || self.Speed == 0
                || self.LastFrame == self.CurrFrame)
                return;
            self.LastFrame = self.CurrFrame;
            var unit = self.GetParent<Unit>();
            var currTarget = self.CurrPath[self.CurrPathIndex];
            if (self.CurrFrame >= self.StartMoveFrame + self.NeedFrame)
            {
                Log.Debug($"Unit 移动结束 Frame{self.CurrFrame}  S {self.StartMoveFrame} N {self.NeedFrame} \n S {self.StartPos} C {currTarget} P {unit.Position}");
                //本次移动结束
                unit.Position = currTarget;
                // 最后一个点的处理
                if (self.CurrPathIndex >= self.CurrPath.Count - 1)
                {
                    self.Stop( MoveStopError.OK);
                    return;
                }
                // 切换到下个点
                self.SetNextTarget();
                return;
            }
            
            // 开始移动
            var target = Vector3.Lerp(self.StartPos, currTarget, ((float) (self.CurrFrame - self.StartMoveFrame)) / self.NeedFrame);
            unit.Position = target;
            //Log.Debug($"Unit 移动中 S {self.StartPos} C {currTarget} P {unit.Position} \n C {self.CurrFrame} S {self.StartMoveFrame} N {self.NeedFrame}");
        }

        static void SetNextTarget(this FrameMoveComponent self)
        {
            var unit = self.GetParent<Unit>();
            self.CurrPathIndex++;
            var currTarget = self.CurrPath[self.CurrPathIndex];
            self.StartMoveFrame = self.CurrFrame;
            self.StartPos = unit.Position;
            var realTime = Vector3.Distance(currTarget, unit.Position) / self.Speed;
            self.NeedFrame = (int) (realTime * 1000 / Game.ClientFrameDuration);
        }

        public static void Stop(this FrameMoveComponent self, MoveStopError stopError)
        {
            self.CurrPathIndex = 0;
            self.CurrPath.Clear();
            self.StartMoveFrame = 0;
            self.StartPos = default;
            self.NeedFrame = 0;
            var tcs = self.MoveTcs;
            self.MoveTcs = null;
            tcs?.SetResult(stopError);
        }

        public static bool IsArrived(this FrameMoveComponent self)
        {
            return self.CurrPathIndex == 0;
        }

        public static async ETTask<MoveStopError> Move(this FrameMoveComponent self, List<Vector3> Path, float speed)
        {
            self.Stop(MoveStopError.Cancel);
            self.CurrPath.AddRange(Path);
            self.Speed = speed;
            self.SetNextTarget();
            self.MoveTcs = new ETTaskCompletionSource<MoveStopError>();
            return await self.MoveTcs.Task;
        }

        public static async ETTask<MoveStopError> MoveTo(this FrameMoveComponent self, Vector3 target, float speed)
        {
            self.Stop(MoveStopError.Cancel);
            self.CurrPath.Add(self.GetParent<Unit>().Position);
            self.CurrPath.Add(target);
            self.Speed = speed;
            self.SetNextTarget();
            self.MoveTcs = new ETTaskCompletionSource<MoveStopError>();
            return await self.MoveTcs.Task;
        }

        public static void Simulate(this FrameMoveComponent self, List<Vector3> path, float speed, int startFrame)
        {
            self.Move(path,speed).Coroutine();
            self.LastFrame = startFrame;
            self.CurrFrame = startFrame;
        }
        public static void RunNext(this FrameMoveComponent self)
        {
            self.CurrFrame = self.GetCurrFrame();
            self.Run();
        }
    }
}
