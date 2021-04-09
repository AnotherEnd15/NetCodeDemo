using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public static class MoveHelper
    {
        // 可以多次调用，多次调用的话会取消上一次的协程
        public static void FindPathMoveToAsync(this Unit unit,int FrameIndex, Vector3 target, ETCancellationToken cancellationToken = null)
        {
            float speed = unit.GetComponent<NumericComponent>().GetAsFloat(NumericType.Speed);
            if (speed < 0.001)
            {
                unit.SendStop(-1);
                return;
            }
            
            using (var list = ListComponent<Vector3>.Create())
            {
                unit.Domain.GetComponent<RecastPathComponent>().SearchPath(10001, unit.Position, target, list.List);

                List<Vector3> path = list.List;
                if (path.Count < 2)
                {
                    unit.SendStop(-2);
                    return;
                }
                unit.CreateFrameInput_Move(FrameIndex,target,path);
            }
        }

        public static void SendStop(this Unit unit, int error)
        {
            MessageHelper.Broadcast(unit, new M2C_Stop()
            {
                Error = error,
                Id = unit.Id, 
                X = unit.Position.x,
                Y = unit.Position.y,
                Z = unit.Position.z,
						
                A = unit.Rotation.x,
                B = unit.Rotation.y,
                C = unit.Rotation.z,
                W = unit.Rotation.w,
            });
        }
    }
}