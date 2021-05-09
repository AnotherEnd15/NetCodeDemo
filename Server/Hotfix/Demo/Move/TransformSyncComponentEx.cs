using EventIDType;

namespace ET
{
    [Event]
    public class TransformSync_Pos : AEvent<ChangePosition>
    {
        protected override async ETTask Run(ChangePosition data)
        {
            var com = data.Unit.GetComponent<TransformSyncComponent>();
            if (com == null) return;
            com.DirtyPos = true;
            await ETTask.CompletedTask;
        }
    }

    [Event]
    public class Transfrom_Rot: AEvent<ChangeRotation>
    {
        protected override async ETTask Run(ChangeRotation data)
        {
            var com = data.Unit.GetComponent<TransformSyncComponent>();
            if (com == null) return;
            com.DirtyRot = true;
            await ETTask.CompletedTask;
        }
    }
}