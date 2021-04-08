

namespace ET
{
	public class AppStartInitFinish_RemoveLoginUI: AEvent<EventIDType.AppStartInitFinish>
	{
		protected override async ETTask Run(EventIDType.AppStartInitFinish args)
		{
			await UIHelper.Create(args.ZoneScene, UIType.UILogin);
		}
	}
}
