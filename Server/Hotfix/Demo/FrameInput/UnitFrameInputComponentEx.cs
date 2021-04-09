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
        public static void Handle(this UnitFrameInputComponent self)
        {
            foreach (FrameInput v in self.Children.Values)
            {
                v.Run();
            }
        }
    }
}