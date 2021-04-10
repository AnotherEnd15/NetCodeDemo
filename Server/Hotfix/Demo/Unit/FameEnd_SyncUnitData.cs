using EventIDType;

namespace ET
{
    [Event]
    public class FameEnd_SyncUnitData : AEvent<EventIDType.FrameEnd>
    {
        protected override async ETTask Run(FrameEnd a)
        {
            HandleTransfrom(a.unit);
            HandleInput(a.unit);
            await ETTask.CompletedTask;
        }

        void HandleTransfrom(Unit unit)
        {
            var com = unit.GetComponent<TransformSyncComponent>();
            if (com == null) return;
            if (!com.DirtyPos && !com.DirtyRot) return;
            var proto = new UpdateTransformProto()
            {
                Dir = com.DirtyRot? unit.Forward.ToOpV3() : null, Pos = com.DirtyPos? unit.Position.ToOpV3() : null, UnitId = unit.Id
            };
            foreach (var v in unit.GetAOIPlayers())
            {
                var diryCom = v.GetComponent<UnitDiryDataComponent>();
                diryCom.Transforms.Add(proto);
            }
        }

        void HandleInput(Unit unit)
        {
            if (unit.Config.Type != (int) UnitType.Player)
                return;
            var com = unit.GetComponent<FrameInputResultComponent>();
            var diryCom = unit.GetComponent<UnitDiryDataComponent>();
            {
                if (com.HasMoveInput)
                {
                    diryCom.MoveInputResultProto = new MoveInputProto();
                    diryCom.MoveInputResultProto.Vaild = com.ValidMove;
                    if (com.ValidMove)
                        diryCom.MoveInputResultProto.Target = com.MoveTarget.ToOpV3();
                }
                else
                {
                    diryCom.MoveInputResultProto = null;
                }
            }
            
            com.Clear();
        }
    }
}