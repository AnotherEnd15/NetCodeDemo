using UnityEngine.Rendering;

namespace ET
{
    [MessageHandler]
    public class M2C_UpdateFrameHandler : AMHandler<M2C_UpdateFrame>
    {
        protected override async ETVoid Run(Session session, M2C_UpdateFrame message)
        {
            var clientFrame = message.Frame * Game.ServerFrameDuration / Game.ClientFrameDuration;
            FrameTimeHelper.SetServerFrame(session, clientFrame);

            var com = session.ZoneScene().GetComponent<SceneDirtyDataComponent>();
            com.Units.AddRange(message.Units);
            com.Transforms.AddRange(message.Transforms);
            if (message.InputResult != null)
                com.MoveInputResult = message.InputResult.Move;
            com.MyUnitId = message.MyUnitId;
            await ETTask.CompletedTask;
        }
    }
}