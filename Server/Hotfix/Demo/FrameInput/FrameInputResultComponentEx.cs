using MongoDB.Driver.Core.Events;
using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class FrameInputResultComponentDestroySystem: DestroySystem<FrameInputResultComponent>
    {
        public override void Destroy(FrameInputResultComponent self)
        {
            self.Clear();
        }
    }
    

    public static class FrameInputResultComponentEx
    {
        public static void Clear(this FrameInputResultComponent self)
        {
            self.ValidMove = default;
            self.MoveTarget = default;
            self.HasMoveInput = default;
            self.ClientFrame = 0;
        }

        public static void SetMove(this FrameInputResultComponent self,int clientFrame, bool valid, Vector3 target)
        {
            self.ValidMove = valid;
            self.MoveTarget = target;
            self.HasMoveInput = true;
            self.ClientFrame = clientFrame;
        }
        
    }
}