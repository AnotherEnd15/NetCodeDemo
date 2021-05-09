using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public static class MoveHelper
    {
        // 可以多次调用，多次调用的话会取消上一次的协程
        public static void FindPathMoveToAsync(this Unit unit,int FrameIndex, Vector3 target, ETCancellationToken cancellationToken = null)
        {
            var nextFrame = unit.GetCurrFrame() + 1;
            Log.Debug($"Client: {FrameIndex}  Server: {nextFrame}");
            if (FrameIndex < nextFrame)
            {
                //客户端因为网络原因,导致上传的输入延迟了,目前是kcp,消息有序到达,那么服务器复制这个输入
                FrameIndex = nextFrame;
                return;
            }
            // float speed = unit.GetComponent<NumericComponent>().GetAsFloat(NumericType.Speed);
            // if (speed < 0.001)
            // {
            //     unit.GetComponent<FrameInputResultComponent>().SetMove(false,default);
            //     return;
            // }
            
            using (var list = ListComponent<Vector3>.Create())
            {
                unit.Domain.GetComponent<RecastPathComponent>().SearchPath(10001, unit.Position, target, list.List);

                List<Vector3> path = list.List;
                if (path.Count < 2)
                {
                    unit.GetComponent<FrameInputResultComponent>().SetMove(false,default);
                    return;
                }
                unit.CreateFrameInput_Move(FrameIndex,target,path);
                unit.GetComponent<FrameInputResultComponent>().SetMove(true,target);
            }
        }
        
    }
}