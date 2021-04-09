using UnityEngine;

namespace ET
{
    // 每帧开始的时候,获取到的玩家输入请求
    public class FrameInput : Entity
    {
        public Vector3 MoveDir; // 往哪个方向走,前方?还是后方?
    }
}