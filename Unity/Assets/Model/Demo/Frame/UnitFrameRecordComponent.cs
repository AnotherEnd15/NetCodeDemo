using System.Collections.Generic;

namespace ET
{
    // 玩家历史帧状态记录
    public class UnitFrameRecordComponent : Entity
    {
        // 使用一个环形list来处理缓存的帧输入,最大缓存一定帧数(和服务器确定帧相差到达一定程度后就不模拟了)
        public CircularList<UnitFrame> AllFrames = new CircularList<UnitFrame>(SceneFrameManagerComponent.MaxForecastFrame + 1); // 多一帧是确定的帧
    }
}