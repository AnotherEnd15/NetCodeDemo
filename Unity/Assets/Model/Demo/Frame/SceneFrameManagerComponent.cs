namespace ET
{
    // 场景中管理帧运行的组件
    public class SceneFrameManagerComponent : Entity
    {
        // 最大缓存帧数
        public const int MaxForecastFrame = 60;
        public const int UpdateDelayFrame = 2 * Game.ServerFrameDuration / Game.ClientFrameDuration; // 客户端其他单位落后帧数
        
        public int CurrSimulateFrame; // 当前模拟的帧, 一般比服务器的当前帧快半个RTT+1帧缓存的时间
        public int LastServerFrame; // 最后确定的服务器帧
        public int SimulateServerFrame; // 根据最后确定的服务器帧模拟的当前帧
        
        public long frameTimer;
    }
}