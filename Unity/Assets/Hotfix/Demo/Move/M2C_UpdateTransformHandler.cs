using UnityEngine;

namespace ET
{
	[MessageHandler]
	public class M2C_UpdateTransformHandler : AMHandler<M2C_UpdateTransform>
	{
		protected override async ETVoid Run(Session session, M2C_UpdateTransform message)
		{
			Unit unit = session.Domain.GetComponent<UnitComponent>().Get(message.Id);

			float speed = unit.GetComponent<NumericComponent>().GetAsFloat(NumericType.Speed);

			using var list = ListComponent<Vector3>.Create();
			
			await unit.GetComponent<FrameMoveComponent>().MoveToAsync(list.List, speed);
		}
	}
}
