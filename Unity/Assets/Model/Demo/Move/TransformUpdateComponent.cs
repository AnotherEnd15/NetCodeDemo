using UnityEngine;

namespace ET
{
    public class TransformUpdateComponent : Entity
    {
        public int EndServerFrame; // 结束同步的帧
        public Vector3 TargetPos;
        public Vector3 TargetDir;
        public int StartFrame;
        public Vector3 StartPos;

        public long frameTimer;
    }
}