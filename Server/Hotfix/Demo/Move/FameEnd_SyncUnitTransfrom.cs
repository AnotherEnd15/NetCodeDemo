using EventIDType;

namespace ET
{
    [Event]
    public class FameEnd_SyncUnitTransfrom : AEvent<EventIDType.FrameEnd>
    {
        protected override async ETTask Run(FrameEnd a)
        {
            var com = a.unit.GetComponent<TransformSyncComponent>();
            if (com == null) return;
            if (!com.DirtyPos && !com.DirtyRot) return;
            var msg = new M2C_UpdateTransform();
            msg.Frame = com.GetCurrFrame();
            if (com.DirtyPos)
            {
                msg.Pos = a.unit.Position.ToOpV3();
                com.DirtyPos = false;
            }
            if (com.DirtyRot)
            {
                msg.Dir = a.unit.Forward.ToOpV3();
                com.DirtyRot = false;
            }
            MessageHelper.Broadcast(a.unit,msg);
            await ETTask.CompletedTask;
        }
    }
}