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
            var com = session.ZoneScene().GetComponent<SceneDirtyDataComponent>();
            com.CurrServerFrame = clientFrame;
            com.Units.AddRange(message.Units);

            com.Transforms.AddRange(message.Transforms);
            if (message.InputResult != null)
                com.MoveInputResult = message.InputResult.Move;
            com.MyUnitId = message.MyUnitId;
            if (message.RemoveUnits.Count > 0)
            {
                com.RemoveUnits.AddRange(message.RemoveUnits);
            }

            await ETTask.CompletedTask;
        }
    }
}