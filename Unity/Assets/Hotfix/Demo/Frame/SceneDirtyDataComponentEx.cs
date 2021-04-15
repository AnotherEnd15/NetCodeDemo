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
            
        }

        public static void Handle(this SceneDirtyDataComponent com)
        {
            var lastServerFrame = com.GetLastServerFrame();
            //1. 先创建Unit
            foreach (var v in com.Units)
            {
                UnitFactory.Create(com.Domain, v, com.MyUnitId);
            }
            com.Units.Clear();
            //2.更新状态
            foreach (var v in com.Transforms)
            {
                var unit = com.Domain.GetComponent<UnitComponent>().Get(v.UnitId);
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
            com.Transforms.Clear();
            //3.todo 更新数值和各种状态
            //4.确定是否有预测错误, 如果有错误,则回滚自己(获取回到正确时间的位置,然后重新执行所有输入,直到当前(上一帧))
            var myUnit = com.Domain.GetComponent<UnitComponent>().MyUnit;
            if (com.MoveInputResult != null && !com.MoveInputResult.Vaild)
            {
                var record = myUnit.GetComponent<UnitFrameRecordComponent>().AllFrames[lastServerFrame];
                myUnit.Position = record.Pos;
                myUnit.Forward = record.Dir;
                var speed = myUnit.GetComponent<NumericComponent>().GetAsFloat(NumericType.Speed);
                for (int i = lastServerFrame; i < com.GetCurrSimulateFrame(); i++)
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

            com.MoveInputResult = null;
            
            
            com.Clear();

        }
    }
}