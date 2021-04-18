namespace ET
{
    public static class FrameInput_MoveEx
    {
        public static async ETVoid Run(this FrameInput_Move self,Unit unit)
        {
            var speed = unit.GetComponent<NumericComponent>().GetAsFloat(NumericType.Speed);
            await unit.GetComponent<FrameMoveComponent>().Move(self.Path, speed);
        }
        
    }
}