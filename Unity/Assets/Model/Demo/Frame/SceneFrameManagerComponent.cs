namespace ET
{
    // 场景中管理帧运行的组件
    public class SceneFrameManagerComponent : Entity
    {
        public int CurrSimulateFrame; // 当前模拟的帧, 一般比服务器的当前帧快半个RTT+1帧缓存的时间
        public int LastServerFrame; // 最后确定的服务器帧
        
        public long frameTimer;
    }
}