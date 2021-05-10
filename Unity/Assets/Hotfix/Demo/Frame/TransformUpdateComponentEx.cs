using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class TransformUpdateComponentDestorySystem: DestroySystem<TransformUpdateComponent>
    {
        public override void Destroy(TransformUpdateComponent self)
        {
            self.Domain.GetComponent<FrameTimerComponent>()?.Remove(self.frameTimer);
            self.frameTimer = 0;
        }
    }

    public static class TransformUpdateComponentEx
    {
        static void SetTimer(this TransformUpdateComponent self)
        {
            var frameTimerCom = self.Domain.GetComponent<FrameTimerComponent>();
            frameTimerCom.Remove(self.frameTimer);
            self.frameTimer = frameTimerCom.NewRepeatedFrameTimer(1, self.Run);
        
            self.StartFrame = self.GetSimulateServerFrame();
            // 位置改变应该在这落后的2帧内插值完成
            self.EndServerFrame = self.StartFrame + SceneFrameManagerComponent.UpdateDelayFrame;
            Log.Debug($"其他单位 准备移动 {self.StartFrame}  {self.EndServerFrame}  {self.TargetPos}");
        }

        static void Run(this TransformUpdateComponent self)
        {
            var currFrame = self.GetSimulateServerFrame();
            if (currFrame > self.EndServerFrame)
            {
                var frameTimerCom = self.Domain.GetComponent<FrameTimerComponent>();
                frameTimerCom.Remove(self.frameTimer);
                return;
            }
            var pos = Vector3.Lerp(self.StartPos, self.TargetPos,
                ((float) (currFrame - self.StartFrame)) / SceneFrameManagerComponent.UpdateDelayFrame);
            //Log.Debug($"其他单位 开始移动 {pos} {self.StartFrame}  {currFrame}");
            self.GetParent<Unit>().Position = pos;
        }

        public static void SetPosTarget(this TransformUpdateComponent self, Vector3 target)
        {
            self.TargetPos = target;
            self.StartPos = self.GetParent<Unit>().Position;
            self.SetTimer();
        }

  
    }
}