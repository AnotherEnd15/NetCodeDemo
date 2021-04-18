using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace ET
{
    public static class MoveHelper
    {
        public static List<Vector3> CalPath(this Unit unit, Vector3 targetPos)
        {
            var path = new NavMeshPath();
            UnityEngine.AI.NavMesh.CalculatePath(unit.Position, targetPos, NavMesh.AllAreas, path);
            if (path.corners == null
                || path.corners.Length <= 1)
                return null;
            return path.corners.ToList();
        }
    }
}