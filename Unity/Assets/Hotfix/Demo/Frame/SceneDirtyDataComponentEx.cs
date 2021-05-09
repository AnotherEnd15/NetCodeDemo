using MongoDB.Bson.Serialization.Serializers;
using UnityEngine;

namespace ET
{
    public static class SceneDirtyDataComponentEx
    {
        public static void Clear(this SceneDirtyDataComponent self)
        {
            self.MoveInputResult = null;
            self.Transforms.Clear();
            self.Units.Clear();
            self.RemoveUnits.Clear();
        }

        public static void Handle(this SceneDirtyDataComponent com)
        {
            var lastServerFrame = com.Domain.GetComponent<SceneFrameManagerComponent>().LastServerFrame;
            //1. 先创建Unit, 删除Unit
            foreach (var v in com.Units)
            {
                UnitFactory.Create(com.Domain, v, com.MyUnitId);
            }
            com.Units.Clear();
            foreach (var v in com.RemoveUnits)
            {
                com.Domain.GetComponent<UnitComponent>().Remove(v);
            }
            com.RemoveUnits.Clear();
            
            var myUnit = com.Domain.GetComponent<UnitComponent>().MyUnit;
            if (com.Transforms.Count > 0)
            {
                //2.更新状态
                foreach (var v in com.Transforms)
                {
                    var unit = com.Domain.GetComponent<UnitComponent>().Get(v.UnitId);
                    Vector3 recordPos = unit.Position;
                    Vector3 recordDir = unit.Forward;
                    if (v.UnitId != com.MyUnitId)
                    {
                        if (v.Pos != null)
                            unit.GetComponent<TransformUpdateComponent>().SetPosTarget(v.Pos.ToV3());
                        if (v.Dir != null)
                            unit.Forward = v.Dir.ToV3(); // 角度先暂时直接转向吧
                        recordPos = unit.Position;
                        recordDir = unit.Forward;
                    }
                    else
                    {
                        var lastRecord = unit.GetComponent<UnitFrameRecordComponent>().AllFrames[lastServerFrame - 1];
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

                    unit.GetComponent<UnitFrameRecordComponent>().AddRecord(lastServerFrame, recordPos, recordDir);

                }
            }
            else
            {
                myUnit.GetComponent<UnitFrameRecordComponent>().AddRecord(lastServerFrame, myUnit.Position, myUnit.Forward);
            }

            com.Transforms.Clear();
            //3.todo 更新数值和各种状态
            //4.确定是否有预测错误,进行预测错误的处理

            if (com.MoveInputResult != null && !com.MoveInputResult.Vaild)
            {
                var targetFrame = com.MoveInputResult.ClientFrame;
                if (targetFrame >= lastServerFrame)
                {
                    var record = myUnit.GetComponent<UnitFrameRecordComponent>().AllFrames[lastServerFrame];

                    var targetPos = myUnit.Position;
                    var targetDir = myUnit.Forward;

                    myUnit.Position = record.Pos;
                    myUnit.Forward = record.Dir;
                    //todo: 应该要获取当时的速度
                    var speed = myUnit.GetComponent<NumericComponent>().GetAsFloat(NumericType.Speed);

                    // 有两种方案
                    // 1. 回滚到服务器确认的帧位置,然后重播所有到现在的输入,计算一个最终的位置和角度
                    // 2. 以当前位置为玩家期望位置,从服务器确认的帧位置往当前期望位置插值 (当前采用的. 因为每次发给服务器的移动输入的时候,其实传入的就是期望位置)

                    var path = myUnit.CalPath(targetPos);
                    if (path != null)
                    {
                        myUnit.GetComponent<FrameMoveComponent>().Simulate(path, speed, targetFrame);
                        for (int i = targetFrame + 1; i < com.GetCurrSimulateFrame(); i++)
                        {
                            myUnit.GetComponent<FrameMoveComponent>().RunNext(i);
                        }
                    }
                }
            }

            com.MoveInputResult = null;
            
            
            com.Clear();

        }
    }
}