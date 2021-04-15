using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class UnitDiryDataComponentDestroySystem: DestroySystem<UnitDiryDataComponent>
    {
        public override void Destroy(UnitDiryDataComponent self)
        {
            self.Numerics.Clear();
            self.Transforms.Clear();
        }
    }
    
    

    public static class UnitDiryDataComponentEx
    {
        public static void Clear(this UnitDiryDataComponent self)
        {
            self.Numerics.Clear();
            self.Transforms.Clear();
            self.MoveInputResultProto = null;
            self.Units.Clear();
        }

        public static void Handle(this UnitDiryDataComponent self, int CurrFrame)
        {
            var msg = new M2C_UpdateFrame();
            msg.Frame = CurrFrame;
            msg.MyUnitId = self.GetParent<Unit>().Id;
            msg.Transforms.AddRange(self.Transforms);
            msg.Numerics.AddRange(self.Numerics);
            if (self.MoveInputResultProto != null)
            {
                msg.InputResult = new UpdateInputResultProto();
                msg.InputResult.Move = self.MoveInputResultProto;
            }
            msg.Units.AddRange(self.Units);
            MessageHelper.SendClient(self.GetParent<Unit>(),msg);
            self.Clear();
        }
    }
}