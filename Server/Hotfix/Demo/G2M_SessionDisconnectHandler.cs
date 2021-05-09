

namespace ET
{
	[ActorMessageHandler]
	public class G2M_SessionDisconnectHandler : AMActorLocationHandler<Unit, G2M_SessionDisconnect>
	{
		protected override async ETTask Run(Unit unit, G2M_SessionDisconnect message)
		{
			// 告诉周围玩家,自己没了
			foreach (var v in unit.GetAOIPlayers())
			{
				if(v.Id == unit.Id) continue;
				v.GetComponent<UnitDiryDataComponent>().RemoveUnits.Add(unit.Id);
			}
			unit.Domain.GetComponent<UnitComponent>().Remove(unit.Id);
			await ETTask.CompletedTask;
		}
	}
}