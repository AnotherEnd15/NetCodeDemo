using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class FrameInputDestroySystem: DestroySystem<FrameInput>
    {
        public override void Destroy(FrameInput self)
        {
        }
    }
    
    public static class FrameInputEx
    {
        public static void Run(this FrameInput self)
        {
            var unit = self.Parent.GetParent<Unit>();
            //todo: 做分发
            self.GetComponent<FrameInput_Move>()?.Run(unit);
        }

        public static void CreateFrameInput_Move(this Unit unit, Vector3 Target, List<Vector3> Path)
        {
            var com = unit.GetComponent<UnitFrameInputComponent>();
            if(com == null)
                com =  unit.AddComponent<UnitFrameInputComponent>();
            var frameInput = EntityFactory.CreateWithParent<FrameInput>(com);
            var move = frameInput.AddComponent<FrameInput_Move>();
            move.Path.AddRange(Path);
            move.Target = Target;
            
        }
    }
}