using System;
using System.Data.SqlClient;
using UnityEngine;

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
            self.frameTimer = TimerComponent.Instance.NewRepeatedTimer(Game.ClientFrameDuration, self.RunNextFrame);
        }
    }

    public static class SceneFrameManagerComponentEx
    {
        public static void RunNextFrame(this SceneFrameManagerComponent self)
        {
            var com = self.ZoneScene().GetComponent<SceneDirtyDataComponent>();
            if (com.CurrServerFrame == 0) return;
            FrameTimeHelper.SetServerFrame(self, (int) com.CurrServerFrame);
            com.Handle();
            // 驱动帧时间组件刷新, 世界状态进行变化. 这个是落后服务器2帧的
            // 网络良好时,这个值稳定落后2帧左右.但是网路出现丢包,那么这个值自增速度就可能会落后于lastServerFrame改变速度了
            if (self.SimulateServerFrame < self.LastServerFrame - SceneFrameManagerComponent.UpdateDelayFrame)
            {
                self.SimulateServerFrame++;
                self.Domain.GetComponent<FrameTimerComponent>().Update(self.SimulateServerFrame);
            }

            var myUnit = self.Domain.GetComponent<UnitComponent>().MyUnit;
            
            // 目前这样会导致延迟过大时(>250ms) 这个客户端模拟机制退化,
            // 延迟增加时,lastServerFrame变化速度降低,currSimlateFrame会逐渐接近250ms上限
            // 延迟降低时,lastServerFrame会比currsimluateFrame更快的速度增加
            // 最终self.currSimulateFrame和lastServerFrame保持一个相对稳定的距离
            
            //网络出现丢包时, lastServerFrame变化速度有可能超过当前模拟帧数
            bool needForecast = self.CurrSimulateFrame < self.LastServerFrame + SceneFrameManagerComponent.MaxForecastFrame;
            //最多模拟帧数
            if (needForecast)
            {
                self.CurrSimulateFrame++;
                // 从输入缓冲区中获取自己的输入.然后模拟执行
                myUnit.GetComponent<UnitFrameInputComponent>().Handle(self.CurrSimulateFrame);
                myUnit.GetComponent<FrameMoveComponent>()?.RunNext(self.CurrSimulateFrame);
            }


        }
    }
}