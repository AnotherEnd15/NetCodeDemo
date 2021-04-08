using UnityEngine;

namespace ET
{
    public class LoadingBeginEvent_CreateLoadingUI : AEvent<EventIDType.LoadingBegin>
    {
        protected override async ETTask Run(EventIDType.LoadingBegin args)
        {
            await UIHelper.Create(args.Scene, UIType.UILoading);
        }
    }
}
