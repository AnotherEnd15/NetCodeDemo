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
            if (self.AllInputs[frame] == null)
            {
                self.AllInputs[frame] = EntityFactory.CreateWithParentAndId<FrameInput>(self, frame);
            }
            return self.AllInputs[frame];
        }

        public static void Handle(this UnitFrameInputComponent self,int frame)
        {
            self.AllInputs[frame]?.Run();
        }

        public static FrameInput CreateInput_Move(this UnitFrameInputComponent self,Vector3 target,List<Vector3> path)
        {
            int frame = self.GetCurrSimulateFrame() + 1;
            var frameInput = self.CreateOrGet(frame);
            frameInput.RemoveComponent<FrameInput_Move>();
            var move = frameInput.AddComponent<FrameInput_Move>();
            move.Target = target;
            move.Path.AddRange(path);
            return frameInput;
        }
    }
}