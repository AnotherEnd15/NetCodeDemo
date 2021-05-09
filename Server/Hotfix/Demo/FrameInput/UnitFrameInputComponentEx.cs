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


        public static bool CheckCanAdd(this UnitFrameInputComponent self, int frame)
        {
            if (frame - self.GetCurrFrame() >= self.AllInputs.Capacity) return false;
            return true;
        }
    }
}