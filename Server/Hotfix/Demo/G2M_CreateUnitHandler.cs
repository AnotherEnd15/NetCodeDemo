using System;
using UnityEngine;

namespace ET
{
	[ActorMessageHandler]
	public class G2M_CreateUnitHandler : AMActorRpcHandler<Scene, G2M_CreateUnit, M2G_CreateUnit>
	{
		protected override async ETTask Run(Scene scene, G2M_CreateUnit request, M2G_CreateUnit response, Action reply)
		{
			Unit unit = EntityFactory.CreateWithId<Unit, int>(scene, IdGenerater.Instance.GenerateId(), 1001);
			unit.AddComponent<FrameMoveComponent>();
			unit.AddComponent<UnitFrameInputComponent>();
			unit.AddComponent<FrameInputResultComponent>();
			unit.AddComponent<TransformSyncComponent>();
			var myDirtyCom = unit.AddComponent<UnitDiryDataComponent>();
			
			unit.Position = new Vector3(-10, 0, -10);
			
			NumericComponent numericComponent = unit.AddComponent<NumericComponent>();
			numericComponent.Set(NumericType.Speed, 6f); // 速度是6米每秒
			
			unit.AddComponent<MailBoxComponent>();
			await unit.AddLocation();
			unit.AddComponent<UnitGateComponent, long>(request.GateSessionId);
			scene.GetComponent<UnitComponent>().Add(unit);
			response.UnitId = unit.Id;
			
			// 周围单位告诉自己

			var units = scene.GetComponent<UnitComponent>().idUnits;
			foreach (Unit u in units.Values)
			{
				myDirtyCom.Units.Add(UnitHelper.CreateUnitInfo(u));
			}
			
			// 把自己告诉周围玩家
			foreach (var v in unit.GetAOIPlayers())
			{
				if(v.Id == unit.Id) continue;
				v.GetComponent<UnitDiryDataComponent>().Units.Add(UnitHelper.CreateUnitInfo(unit));
			}
			
			reply();
		}
	}
}