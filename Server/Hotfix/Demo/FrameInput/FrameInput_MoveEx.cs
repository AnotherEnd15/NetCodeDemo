namespace ET
{
    public static class FrameInput_MoveEx
    {
        public static async ETVoid Run(this FrameInput_Move self,Unit unit)
        {
            var speed = unit.GetComponent<NumericComponent>().GetAsFloat(NumericType.Speed);
            bool ret = await unit.GetComponent<FrameMoveComponent>().MoveToAsync(self.Path, speed);
            if (ret) // 如果返回false，说明被其它移动取消了，这时候不需要通知客户端stop
            {
                unit.SendStop(0);
            }
        }
        
    }
}