using System.Collections.Generic;
using UnityEngine;

namespace ET
{
	[ActorMessageHandler]
	public class C2M_PathfindingResultHandler : AMActorLocationHandler<Unit, C2M_PathfindingResult>
	{
		protected override async ETTask Run(Unit unit, C2M_PathfindingResult message)
		{
			var serverFrame = FrameTimeHelper.ClientFrame2ServerFrame(message.ClientFrame);
			if (!unit.GetComponent<UnitFrameInputComponent>().CheckCanAdd(serverFrame))
			{
				return;
			}
			Vector3 target = message.Target.ToV3();
			unit.FindPathMoveToAsync(serverFrame,message.ClientFrame, target);
			await ETTask.CompletedTask;
		}
	}
}