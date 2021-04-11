using System.Collections.Generic;
using UnityEngine;

namespace ET
{
	[ActorMessageHandler]
	public class C2M_PathfindingResultHandler : AMActorLocationHandler<Unit, C2M_PathfindingResult>
	{
		protected override async ETTask Run(Unit unit, C2M_PathfindingResult message)
		{
			Vector3 target = message.Target.ToV3();
			unit.FindPathMoveToAsync(message.Frame,target);
			await ETTask.CompletedTask;
		}
	}
}