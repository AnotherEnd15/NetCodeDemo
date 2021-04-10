using System.Collections.Generic;

namespace ET
{
    // 客户端自己的预表现
    public class UnitFrameInputComponent : Entity
    {
        // 使用一个环形list来处理缓存的帧输入,最大缓存一定帧数(和服务器确定帧相差到达一定程度后就不模拟了)
        public CircularList<FrameInput> AllInputs = new CircularList<FrameInput>(60);
        public int LastServerFrame; // 最后一个服务器确定的帧
    }
}