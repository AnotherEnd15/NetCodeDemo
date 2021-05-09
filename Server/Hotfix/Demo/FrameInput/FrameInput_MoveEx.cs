using System.Linq;

namespace ET
{
    public static class FrameInput_MoveEx
    {
        public static async ETVoid Run(this FrameInput_Move self,Unit unit)
        {
            var speed = unit.GetComponent<NumericComponent>().GetAsFloat(NumericType.Speed);
            Log.Debug($"根据输入移动  {speed}  {unit.Position}  {self.Path.Last()}");
            await unit.GetComponent<FrameMoveComponent>().Move(self.Path, speed);
        }
        
    }
}