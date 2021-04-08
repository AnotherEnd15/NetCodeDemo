namespace ET
{
    public class AfterCreateZoneScene_AddComponent: AEvent<EventIDType.AfterCreateZoneScene>
    {
        protected override async ETTask Run(EventIDType.AfterCreateZoneScene args)
        {
            Scene zoneScene = args.ZoneScene;
            zoneScene.AddComponent<UIEventComponent>();
            zoneScene.AddComponent<UIComponent>();
            await ETTask.CompletedTask;
        }
    }
}