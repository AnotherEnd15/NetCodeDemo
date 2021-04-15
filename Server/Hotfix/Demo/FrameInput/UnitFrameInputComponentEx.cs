namespace ET
{
    [ObjectSystem]
    public class UnitFrameInputComponentDestroySystem: DestroySystem<UnitFrameInputComponent>
    {
        public override void Destroy(UnitFrameInputComponent self)
        {
            self.AllInputs.Clear();
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

        public static bool CheckCanAdd(this UnitFrameInputComponent self, int frame)
        {
            if (frame - self.GetCurrFrame() >= self.AllInputs.Capacity) return false;
            return true;
        }
    }
}