using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public static class MoveHelper
    {
        // 可以多次调用，多次调用的话会取消上一次的协程
        public static void FindPathMoveToAsync(this Unit unit,int FrameIndex,int clientFrame, Vector3 target,List<OpVector3> path, ETCancellationToken cancellationToken = null)
        {
            var nextFrame = unit.GetCurrFrame() + 1;
            Log.Debug($"Client: {FrameIndex}  Server: {nextFrame}");
            if (FrameIndex < nextFrame)
            {
                //客户端因为网络原因,导致上传的输入延迟了,目前是kcp,消息有序到达,那么服务器复制这个输入
                FrameIndex = nextFrame;
            }
            // float speed = unit.GetComponent<NumericComponent>().GetAsFloat(NumericType.Speed);
            // if (speed < 0.001)
            // {
            //     unit.GetComponent<FrameInputResultComponent>().SetMove(false,default);
            //     return;
            // }
            
            using (var list = ListComponent<Vector3>.Create())
            {
                //todo: 服务器和客户端目前用的不是同一套寻路,必定有误差. 有机会再改. 现在demo先完全相信客户端
                // if (!unit.Domain.GetComponent<RecastPathComponent>().IsValid(10001, path))
                // {
                //     unit.GetComponent<FrameInputResultComponent>().SetMove(clientFrame,false,target);
                //     return;
                // }

                foreach (var v in path)
                {
                    list.List.Add(v.ToV3());
                }
                unit.CreateFrameInput_Move(FrameIndex,target,list.List);
                unit.GetComponent<FrameInputResultComponent>().SetMove(clientFrame,true,target);
            }
        }
        
    }
}