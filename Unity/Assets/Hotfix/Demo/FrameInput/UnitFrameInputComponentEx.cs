using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class UnitFrameInputComponentDestroySystem: DestroySystem<UnitFrameInputComponent>
    {
        public override void Destroy(UnitFrameInputComponent self)
        {
        }
    }
    

    public static class UnitFrameInputComponentEx
    {
        public static FrameInput CreateOrGet(this UnitFrameInputComponent self, int frame)
        {
            if (!self.Children.TryGetValue(frame,out var Input))
            {
                Input = EntityFactory.CreateWithParentAndId<FrameInput>(self, frame);
            }
            return Input as FrameInput;
        }

        public static void Handle(this UnitFrameInputComponent self,int frame)
        {
            if (!self.Children.TryGetValue(frame, out var Input))
            {
                return;
            }
            (Input as FrameInput).Run();
        }

        public static FrameInput CreateInput_Move(this UnitFrameInputComponent self,Vector3 target,List<Vector3> path)
        {
            int frame = self.GetCurrSimulateFrame() + 1;
            var frameInput = self.CreateOrGet(frame);
            var move = frameInput.AddComponent<FrameInput_Move>();
            move.Target = target;
            move.Path.AddRange(path);
            return frameInput;
        }
    }
}