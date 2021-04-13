using UnityEngine;

namespace ET
{
    public static class UnitFactory
    {
	    public static OpVector3 ToOpV3(this Vector3 v)
	    {
		    return new OpVector3() { X = v.x, Y = v.y, Z = v.z };
	    }
        
	    public static Vector3 ToV3(this OpVector3 v)
	    {
		    return new Vector3() { x = v.X, y = v.Y, z = v.Z };
	    }
	    
        public static Unit Create(Entity domain, UnitInfo unitInfo,long myUnitId)
        {
	        Unit unit = EntityFactory.CreateWithId<Unit, int>(domain, unitInfo.UnitId, unitInfo.ConfigId);
	        unit.Position = unitInfo.Pos.ToV3();
	        bool isMyUnit = unitInfo.UnitId == myUnitId;
	        unit.AddComponent<FrameMoveComponent>();
	        if (isMyUnit)
	        {
		        unit.AddComponent<UnitFrameInputComponent>();
		        unit.AddComponent<UnitFrameRecordComponent>();
	        }

	        NumericComponent numericComponent = unit.AddComponent<NumericComponent>();
	        for (int i = 0; i < unitInfo.KVs.Count; ++i)
	        {
		        numericComponent.Set((NumericType)unitInfo.KVs[i].Key, unitInfo.KVs[i].Value);
	        }

	        UnitComponent unitComponent = domain.GetComponent<UnitComponent>();
            unitComponent.Add(unit);
            if (isMyUnit)
            {
	            unitComponent.MyUnit = unit;
            }

            Game.EventSystem.Publish(new EventIDType.AfterUnitCreate() {Unit = unit});
            return unit;
        }
    }
}
