using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace ET
{
    [ObjectSystem]
    public class OperaComponentAwakeSystem : AwakeSystem<OperaComponent>
    {
        public override void Awake(OperaComponent self)
        {
            self.mapMask = LayerMask.GetMask("Map");
            self.Path = new NavMeshPath();
        }
    }
    [ObjectSystem]
    public class OperaComponentUpdateSystem : UpdateSystem<OperaComponent>
    {
        public override void Update(OperaComponent self)
        {
            self.Update();
        }
    }
    [ObjectSystem]
    public class OperaComponentDestroySystem : DestroySystem<OperaComponent>
    {
        public override void Destroy(OperaComponent self)
        {
            self.Path = null;
        }
    }
    
    public static class OperaComponentSystem
    {
        public static void Update(this OperaComponent self)
        {
            if (Input.GetMouseButtonDown(1))
            {
                var unit = self.Domain.GetComponent<UnitComponent>().MyUnit;
                if (unit == null) return;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000, self.mapMask))
                {
                    self.ClickPoint = hit.point;
                    self.Path.ClearCorners();
                    UnityEngine.AI.NavMesh.CalculatePath(unit.Position, self.ClickPoint, NavMesh.AllAreas, self.Path);
                    if (self.Path.corners == null
                        || self.Path.corners.Length <= 1)
                        return;
                    unit.GetComponent<UnitFrameInputComponent>().CreateInput_Move(self.Path.corners[0], self.Path.corners.ToList());

                }
            }
        }
        
    }
}