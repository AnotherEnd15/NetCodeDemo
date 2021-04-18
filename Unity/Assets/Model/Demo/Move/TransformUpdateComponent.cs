using UnityEngine;

namespace ET
{
    public class TransformUpdateComponent : Entity
    {
        public const int UpdateDelayFrame = 2 * Game.ClientFrameDuration / Game.ServerFrameDuration; // 改变一个单位的状态时,客户端实际上是多少帧后处理 
        public int EndServerFrame; // 结束同步的帧
        public Vector3 TargetPos;
        public Vector3 TargetDir;
        public int StartFrame;
        public Vector3 StartPos;

        public long frameTimer;
    }
}