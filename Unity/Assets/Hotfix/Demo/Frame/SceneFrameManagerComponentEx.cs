﻿using System;
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
            if (self.LastServerFrame == 0) return;
            var com = self.ZoneScene().GetComponent<SceneDirtyDataComponent>();
            com.Handle();
            // 驱动帧时间组件刷新, 世界状态进行变化. 这个是落后服务器2帧的
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
            bool needForecast = self.CurrSimulateFrame < self.LastServerFrame + SceneFrameManagerComponent.MaxForecastFrame;
            //Log.Debug(self.LastServerFrame +"   "+self.CurrSimulateFrame);
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