namespace ET
{
    [ObjectSystem]
    public class SceneFrameManagerComponentDestroySystem: DestroySystem<SceneFrameManagerComponent>
    {
        public override void Destroy(SceneFrameManagerComponent self)
        {
            self.CurrFrame = 0;
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
            self.CurrFrame++;
            //1.从输入缓冲区中获取所有人的当前输入并处理
            foreach (var v in self.Domain.GetComponent<UnitComponent>().GetByType( UnitType.Player))
            {
                v.GetComponent<UnitFrameInputComponent>().Handle();
                v.RemoveComponent<UnitFrameInputComponent>();
            }
            //2.驱动帧时间组件刷新
            self.Domain.GetComponent<FrameTimerComponent>().Update(self.CurrFrame);
        }
    }
}