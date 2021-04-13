using UnityEngine;

namespace ET
{
    public class TransformUpdateComponent : Entity
    {
        public int LastServerFrame; // 确定性位置的帧
        public Vector3 TargetPos;
        public Vector3 TargetDir;

        public long frameTimer;
    }
}