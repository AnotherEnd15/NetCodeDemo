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
            self.StartFrame = self.GetLastServerFrame();
            self.EndServerFrame =  self.StartFrame+TransformUpdateComponent.UpdateDelayFrame;
        }

        static void Run(this TransformUpdateComponent self)
        {
            var currFrame = self.GetLastServerFrame();
            var pos = Vector3.Lerp(self.StartPos, self.TargetPos, currFrame - self.StartFrame / TransformUpdateComponent.UpdateDelayFrame);
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