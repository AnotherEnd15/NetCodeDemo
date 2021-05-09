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
            //todo: 做分发
            self.GetComponent<FrameInput_Move>()?.Run(unit);
        }
        
    }
}