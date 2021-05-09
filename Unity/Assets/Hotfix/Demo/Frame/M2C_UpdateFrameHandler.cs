using UnityEngine.Rendering;

namespace ET
{
    [MessageHandler]
    public class M2C_UpdateFrameHandler : AMHandler<M2C_UpdateFrame>
    {
        protected override async ETVoid Run(Session session, M2C_UpdateFrame message)
        {
            // 服务器一帧展开来是客户端的3帧
            var clientFrame =  (Game.ServerFrameDuration / Game.ClientFrameDuration) * message.Frame  - 1;
            FrameTimeHelper.SetServerFrame(session, (int) clientFrame);

            var com = session.ZoneScene().GetComponent<SceneDirtyDataComponent>();
            com.Units.AddRange(message.Units);
            if (message.Transforms.Count > 0)
            {
                Log.Debug("其他单位位置需要更新  " + message.Transforms.Count);
            }
            com.Transforms.AddRange(message.Transforms);
            if (message.InputResult != null)
                com.MoveInputResult = message.InputResult.Move;
            com.MyUnitId = message.MyUnitId;
            if (message.RemoveUnits.Count > 0)
            {
                Log.Debug("需要移除其他单位  " + message.RemoveUnits.Count);
                com.RemoveUnits.AddRange(message.RemoveUnits);
            }

            await ETTask.CompletedTask;
        }
    }
}