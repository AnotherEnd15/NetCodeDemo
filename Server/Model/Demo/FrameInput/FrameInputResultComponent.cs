using UnityEngine;

namespace ET
{
    // 保持一帧的结果就行,下次发送下去之后会清空
    public class FrameInputResultComponent : Entity
    {
        public Vector3 MoveTarget;
        public bool ValidMove;
        public bool HasMoveInput;
    }
}