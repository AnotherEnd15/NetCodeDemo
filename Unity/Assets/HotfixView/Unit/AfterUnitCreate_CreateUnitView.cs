using UnityEngine;

namespace ET
{
    public class AfterUnitCreate_CreateUnitView: AEvent<EventIDType.AfterUnitCreate>
    {
        protected override async ETTask Run(EventIDType.AfterUnitCreate args)
        {
            // Unit View层
            // 这里可以改成异步加载，demo就不搞了
            GameObject bundleGameObject = (GameObject)ResourcesComponent.Instance.GetAsset("Unit.unity3d", "Unit");
            GameObject prefab = bundleGameObject.Get<GameObject>("Skeleton");
	        
            GameObject go = UnityEngine.Object.Instantiate(prefab, GlobalComponent.Instance.Unit, true);
            go.transform.position = args.Unit.Position;
            args.Unit.AddComponent<GameObjectComponent>().GameObject = go;
            args.Unit.AddComponent<AnimatorComponent>();
            await ETTask.CompletedTask;
        }
    }
    
    public class UnitRemove_CreateUnitView: AEvent<EventIDType.UnitRemove>
    {
        protected override async ETTask Run(EventIDType.UnitRemove args)
        {
            // Unit View层
            // 这里可以改成异步加载，demo就不搞了
            var tarGo = args.Unit.GetComponent<GameObjectComponent>().GameObject;
            GameObject.Destroy(tarGo);
            await ETTask.CompletedTask;
        }
    }
}