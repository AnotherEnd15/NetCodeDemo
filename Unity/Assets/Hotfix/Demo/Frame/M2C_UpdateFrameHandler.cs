using UnityEngine.Rendering;

namespace ET
{
    [MessageHandler]
    public class M2C_UpdateFrameHandler : AMHandler<M2C_UpdateFrame>
    {
        protected override async ETVoid Run(Session session, M2C_UpdateFrame message)
        {
            FrameTimeHelper.SetServerFrame(session,message.Frame);
            
            // 创建Units
            foreach (var v in message.Units)
            {
                var unit =  UnitFactory.Create(session.ZoneScene(), v,message.MyUnitId);
            }
            
            // 更新位置
            foreach (var v in message.Transforms)
            {
                if(v.UnitId == message.MyUnitId) continue;
                var unit = session.ZoneScene().GetComponent<UnitComponent>().Get(v.UnitId);
                //todo: 内插值处理
                if (v.Pos != null)
                    unit.Position = v.Pos.ToV3();
                if (v.Dir != null)
                    unit.Forward = v.Dir.ToV3();
            }
            
            // 验证输入
            if (message.InputResult.Move != null)
            {
                
            }
        }
    }
}