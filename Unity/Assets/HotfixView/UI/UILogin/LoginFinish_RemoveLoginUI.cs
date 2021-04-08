

namespace ET
{
	public class LoginFinish_RemoveLoginUI: AEvent<EventIDType.LoginFinish>
	{
		protected override async ETTask Run(EventIDType.LoginFinish args)
		{
			await UIHelper.Remove(args.ZoneScene, UIType.UILogin);
		}
	}
}
