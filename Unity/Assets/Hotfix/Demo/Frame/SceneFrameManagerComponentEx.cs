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
            //1. 先创建Unit
            foreach (var v in com.Units)
            {
                UnitFactory.Create(self.Domain, v, com.MyUnitId);
            }
            //2.更新状态
            foreach (var v in com.Transforms)
            {
                var unit = self.Domain.GetComponent<UnitComponent>().Get(v.UnitId);
                Vector3 recordPos = unit.Position;
                Vector3 recordDir = unit.Forward;
                if (v.UnitId != com.MyUnitId)
                {
                    if (v.Pos != null)
                        unit.MoveToAsync(v.Pos.ToV3()).Coroutine();
                    if (v.Dir != null)
                        unit.Forward = v.Dir.ToV3(); // 角度先暂时直接转向吧
                    recordPos = unit.Position;
                    recordDir = unit.Forward;
                }
                else
                {
                    var lastRecord = unit.GetComponent<UnitFrameRecordComponent>().AllFrames[self.LastServerFrame - 1];
                    if (lastRecord != null)
                    {
                        recordPos = lastRecord.Pos;
                        recordDir = lastRecord.Dir;
                    }

                    if (v.Pos != null)
                    {
                        recordPos = v.Pos.ToV3();
                    }

                    if (v.Dir != null)
                    {
                        recordDir = v.Dir.ToV3();
                    }
                }

                unit.GetComponent<UnitFrameRecordComponent>().AddRecord(self.LastServerFrame, recordPos, recordDir);

            }
            //3.todo 更新数值和各种状态


            //4.确定是否有预测错误, 如果有错误,则回滚自己(获取回到正确时间的位置,然后重新执行所有输入,直到当前(上一帧))
            var myUnit = self.Domain.GetComponent<UnitComponent>().MyUnit;
            if (com.MoveInputResult != null && !com.MoveInputResult.Vaild)
            {
                var record = myUnit.GetComponent<UnitFrameRecordComponent>().AllFrames[self.LastServerFrame];
                myUnit.Position = record.Pos;
                myUnit.Forward = record.Dir;
                var speed = myUnit.GetComponent<NumericComponent>().GetAsFloat(NumericType.Speed);
                for (int i = self.LastServerFrame; i < self.CurrSimulateFrame; i++)
                {
                    void CheckMove()
                    {
                        //todo: 判断这时候禁止移动buff是否还在,在的话就不管
                        var input = myUnit.GetComponent<UnitFrameInputComponent>().AllInputs[i];
                        if(input == null) return;
                        var inputMove = input.GetComponent<FrameInput_Move>();
                        if(inputMove == null) return;
                    
                        myUnit.GetComponent<FrameMoveComponent>().MoveToAsync(inputMove.Path,speed,true,i).Coroutine();  
                    }

                    CheckMove();
                    myUnit.GetComponent<FrameMoveComponent>().MoveForward(false,i);
 
                }
            }

            com.Clear();
            
            bool needForecast = self.CurrSimulateFrame < self.LastServerFrame + SceneFrameManagerComponent.MaxForecastFrame;
            //最多模拟帧数
            if (needForecast)
            {
                self.CurrSimulateFrame++;
                //6. 驱动帧时间组件刷新, 世界状态进行变化
                self.Domain.GetComponent<FrameTimerComponent>().Update(self.CurrSimulateFrame);
                //7. 从输入缓冲区中获取自己的输入.然后模拟执行
                var input = myUnit.GetComponent<UnitFrameInputComponent>().AllInputs[self.CurrSimulateFrame];
                if (input != null)
                {
                    input.Run();
                }
            }

        }
    }
}