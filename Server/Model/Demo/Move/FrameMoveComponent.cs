using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class FrameMoveComponent: Entity
    {
        public Vector3 MoveTarget; // 当前目标
        public Vector3 StartPos; // 本次移动最开始的点

        public List<Vector3> CurrPath =new List<Vector3>(); 
        public int CurrPathIndex; // 当前路径第几个目标点, 第0点就是自己当前位置,所以为0的时候代表不移动
        public int StartMoveFrame; // 开始移动的帧
        public int NeedFrame; // 预计需要多少帧到达
        public int CurrFrame; // 主角的话,就是当前预测帧,其他人,就是服务器确认帧
        public int LastFrame;// 上一帧, 主要用来判断不继续模拟的情况
        public float Speed; // 当前移动的速度
        public ETTaskCompletionSource<MoveStopError> MoveTcs;

        public long moveTimer;
    }
}