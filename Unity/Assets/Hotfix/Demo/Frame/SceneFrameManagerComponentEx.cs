namespace ET
{
    [ObjectSystem]
    public class SceneFrameManagerComponentDestroySystem: DestroySystem<SceneFrameManagerComponent>
    {
        public override void Destroy(SceneFrameManagerComponent self)
        {
            self.CurrSimulateFrame = 0;
            TimerComponent.Instance.Remove(ref self.frameTimer);
        }
    }

    // [ObjectSystem]
    // public class SceneFrameManagerComponentDeserializeSystem: DeserializeSystem<SceneFrameManagerComponent>
    // {
    //     public override void Deserialize(SceneFrameManagerComponent self)
    //     {
    //     }
    // }
    //
    [ObjectSystem]
    public class SceneFrameManagerComponentStartSystem: StartSystem<SceneFrameManagerComponent>
    {
        public override void Start(SceneFrameManagerComponent self)
        {
            self.frameTimer = TimerComponent.Instance.NewRepeatedTimer(Game.FrameDuration, self.RunNextFrame);
        }
    }

    public static class SceneFrameManagerComponentEx
    {
        public static void RunNextFrame(this SceneFrameManagerComponent self)
        {
            self.CurrSimulateFrame++;
            //1.从输入缓冲区中获取自己的输入
            //2.驱动帧时间组件刷新, 世界状态进行变化
            self.Domain.GetComponent<FrameTimerComponent>().Update(self.CurrSimulateFrame);

                    
        }
    }
}