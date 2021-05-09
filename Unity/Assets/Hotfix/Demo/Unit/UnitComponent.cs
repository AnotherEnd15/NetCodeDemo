using System.Collections.Generic;
using System.Linq;
using EventIDType;

namespace ET
{
	
	public class UnitComponentAwakeSystem : AwakeSystem<UnitComponent>
	{
		public override void Awake(UnitComponent self)
		{
		}
	}
	
	public class UnitComponentDestroySystem : DestroySystem<UnitComponent>
	{
		public override void Destroy(UnitComponent self)
		{
			foreach (Unit unit in self.idUnits.Values)
			{
				unit.Dispose();
			}

			self.idUnits.Clear();
		}
	}
	
	public static class UnitComponentSystem
	{
		public static void Add(this UnitComponent self, Unit unit, bool isMyUnit = false)
		{
			self.idUnits.Add(unit.Id, unit);
			unit.Parent = self;
			if (isMyUnit)
				self.MyUnit = unit;
			Game.EventSystem.Publish(new EventIDType.AfterUnitCreate() { Unit = unit });
		}

		public static Unit Get(this UnitComponent self, long id)
		{
			Unit unit;
			self.idUnits.TryGetValue(id, out unit);
			return unit;
		}

		public static void Remove(this UnitComponent self, long id)
		{
			Unit unit;
			self.idUnits.TryGetValue(id, out unit);
			if (unit != null)
			{
				Game.EventSystem.Publish(new UnitRemove() { Unit = unit });
			}

			self.idUnits.Remove(id);
			unit?.Dispose();
		}

		public static void RemoveNoDispose(this UnitComponent self, long id)
		{
			self.idUnits.Remove(id);
		}

		public static Unit[] GetAll(this UnitComponent self)
		{
			return self.idUnits.Values.ToArray();
		}
	}
}