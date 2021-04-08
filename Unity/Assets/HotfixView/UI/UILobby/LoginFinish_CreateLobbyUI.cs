

namespace ET
{
	public class LoginFinish_CreateLobbyUI: AEvent<EventIDType.LoginFinish>
	{
		protected override async ETTask Run(EventIDType.LoginFinish args)
		{
			await UIHelper.Create(args.ZoneScene, UIType.UILobby);
		}
	}
}
