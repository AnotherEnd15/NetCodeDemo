namespace ET
{
    public static class FrameInput_MoveEx
    {
        public static async ETVoid Run(this FrameInput_Move self,Unit unit)
        {
            var speed = unit.GetComponent<NumericComponent>().GetAsFloat(NumericType.Speed);
            bool ret = await unit.GetComponent<FrameMoveComponent>().MoveToAsync(self.Path, speed);
        }
        
    }
}