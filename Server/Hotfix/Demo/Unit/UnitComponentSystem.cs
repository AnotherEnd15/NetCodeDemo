using System.Collections.Generic;
using System.Linq;

namespace ET
{
    public class UnitComponentAwakeSystem: AwakeSystem<UnitComponent>
    {
        public override void Awake(UnitComponent self)
        {
        }
    }
    
    public class UnitComponentDestroySystem: DestroySystem<UnitComponent>
    {
        public override void Destroy(UnitComponent self)
        {
            self.idUnits.Clear();
        }
    }
    
    public static class UnitComponentSystem
    {
        public static void Add(this UnitComponent self, Unit unit)
        {
            unit.Parent = self;
            self.idUnits.Add(unit.Id, unit);
            var type = unit.Config.Type;
            if (!self.TypeUnits.ContainsKey(type))
                self.TypeUnits[type] = new HashSet<Unit>();
            self.TypeUnits[type].Add(unit);
        }

        public static Unit Get(this UnitComponent self, long id)
        {
            self.idUnits.TryGetValue(id, out Unit unit);
            return unit;
        }

        public static HashSet<Unit> GetByType(this UnitComponent self, UnitType type)
        {
            self.TypeUnits.TryGetValue((int) type, out var v);
            return v;
        }


        public static void Remove(this UnitComponent self, long id, bool dispose = true)
        {
            Unit unit;
            self.idUnits.TryGetValue(id, out unit);
            if (unit == null) return;
            self.idUnits.Remove(id);
            self.TypeUnits[unit.Config.Type].Remove(unit);
            if (dispose)
                unit?.Dispose();
        }
        
    }
}