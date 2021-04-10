namespace ET
{
	public class SessionComponentDestroySystem: DestroySystem<SessionComponent>
	{
		public override void Destroy(SessionComponent self)
		{
			self.Session.Dispose();
		}
	}

	public static class SessionHelper
	{
		public static Session CurrSession(this Entity entity)
		{
			return entity.ZoneScene().GetComponent<SessionComponent>().Session;
		}
	}
}
