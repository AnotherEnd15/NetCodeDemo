namespace ET
{
    public class LoadingFinishEvent_RemoveLoadingUI : AEvent<EventIDType.LoadingFinish>
    {
        protected override async ETTask Run(EventIDType.LoadingFinish args)
        {
            await UIHelper.Create(args.Scene, UIType.UILoading);
        }
    }
}
