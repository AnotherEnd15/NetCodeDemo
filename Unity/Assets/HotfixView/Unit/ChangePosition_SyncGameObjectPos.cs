using UnityEngine;

namespace ET
{
    public class ChangePosition_SyncGameObjectPos: AEvent<EventIDType.ChangePosition>
    {
        protected override async ETTask Run(EventIDType.ChangePosition args)
        {
            GameObjectComponent gameObjectComponent = args.Unit.GetComponent<GameObjectComponent>();
            if (gameObjectComponent == null)
            {
                return;
            }
            Transform transform = gameObjectComponent.GameObject.transform;
            transform.position = args.Unit.Position;
            await ETTask.CompletedTask;
        }
    }
}