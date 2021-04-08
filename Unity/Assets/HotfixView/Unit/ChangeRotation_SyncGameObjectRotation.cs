using UnityEngine;

namespace ET
{
    public class ChangeRotation_SyncGameObjectRotation: AEvent<EventIDType.ChangeRotation>
    {
        protected override async ETTask Run(EventIDType.ChangeRotation args)
        {
            Transform transform = args.Unit.GetComponent<GameObjectComponent>().GameObject.transform;
            transform.rotation = args.Unit.Rotation;
            await ETTask.CompletedTask;
        }
    }
}