using System;

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
            self.frameTimer = TimerComponent.Instance.NewRepeatedTimer(Game.ServerFrameDuration, self.RunNextFrame);
        }
    }

    public static class SceneFrameManagerComponentEx
    {
        public static void AddFrame(this SceneFrameManagerComponent self)
        {
            // 要限制一局游戏不能持续超过1年,正常来讲应该不会. 否则会超出21亿
            self.CurrFrame++;
        }

        public static void RunNextFrame(this SceneFrameManagerComponent self)
        {
            self.AddFrame();
            //1.从输入缓冲区中获取所有人的当前输入并处理, 
            //todo 本地模拟的时候是提前模拟的 (提前1/2 RTT + 1帧),所以这里应该能找到当前帧的输入,找不到就忽略(或者考虑复制之前的输入)
            
            var allPlayers = self.DomainScene().GetComponent<UnitComponent>().GetByType(UnitType.Player);
            if (allPlayers == null) return;
            foreach (var v in allPlayers)
            {
                v.GetComponent<UnitFrameInputComponent>().Handle(self.CurrFrame);
            }
            //2.驱动帧时间组件刷新, 世界状态进行变化
            self.Domain.GetComponent<FrameTimerComponent>().Update(self.CurrFrame);
            
            //3 计算DirtyData 
            foreach (var v in self.Domain.GetComponent<UnitComponent>().idUnits.Values)
            {
                //todo 先这么抛事件处理了
                Game.EventSystem.Publish(new EventIDType.FrameEnd() { unit = v });
            }

            //4 推送数据给玩家
            allPlayers = self.Domain.GetComponent<UnitComponent>().GetByType(UnitType.Player);
            if (allPlayers == null) return;
            foreach (var v in allPlayers)
            {
                v.GetComponent<UnitDiryDataComponent>().Handle(self.CurrFrame);
            }
            
                    
        }
    }
}