using System.Collections.Generic;
using System.Linq;
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

			if (message.Path == null || message.Path.Count == 0 || message.Path.Count>30)
			{
				return;
			}

			message.Path[0] = unit.Position.ToOpV3(); // 修正寻路的第一个路点是玩家自己的当前位置

			Vector3 target = message.Target.ToV3();
			unit.FindPathMoveToAsync(serverFrame,message.ClientFrame, target,message.Path);
			await ETTask.CompletedTask;
		}
	}
}