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
    
    // [ObjectSystem]
    // public class FrameInputAwakeSystem: AwakeSystem<FrameInput,int>
    // {
    //     public override void Awake(FrameInput self,int frameIndex)
    //     {
    //         self.FrameIndex = frameIndex;
    //     }
    // }
    
    public static class FrameInputEx
    {
        public static void Run(this FrameInput self)
        {
            var unit = self.Parent.GetParent<Unit>();
            Log.Debug($"分发输入处理 {self.Id}");
            //todo: 做分发
            self.GetComponent<FrameInput_Move>()?.Run(unit);
        }

        public static void CreateFrameInput_Move(this Unit unit,int frameIndex, Vector3 Target, List<Vector3> Path)
        {
            var com = unit.GetComponent<UnitFrameInputComponent>();
            if(com == null)
                com =  unit.AddComponent<UnitFrameInputComponent>();
            var frameInput = com.CreateOrGet(frameIndex);
            com.AllInputs[frameIndex] = frameInput;
            Log.Debug($"添加帧移动输入 {frameIndex}");
            var move = frameInput.AddComponent<FrameInput_Move>();
            move.Path.AddRange(Path);
            move.Target = Target;
        }
    }
}