using System.Collections.Generic;
using System.Linq;

namespace ET
{
    public static class UnitHelper
    {
        public static UnitInfo CreateUnitInfo(Unit unit)
        {
            UnitInfo unitInfo = new UnitInfo();
            NumericComponent nc = unit.GetComponent<NumericComponent>();
            unitInfo.Pos = unit.Position.ToOpV3();
            unitInfo.Dir = unit.Forward.ToOpV3();
            unitInfo.UnitId = unit.Id;
            unitInfo.ConfigId = unit.ConfigId;

            foreach ((int key, long value) in nc.NumericDic)
            {
                unitInfo.KVs.Add(new IntLongKV()
                {
                    Key = key,
                    Value = value
                });
            }

            return unitInfo;
        }

        public static List<Unit> GetAOIPlayers(this Unit unit)
        {
            return unit.Domain.GetComponent<UnitComponent>().idUnits.Values.ToList();
        }
        
    }
}