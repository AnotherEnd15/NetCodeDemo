using UnityEngine.Rendering;

namespace ET
{
    [MessageHandler]
    public class M2C_UpdateFrameHandler : AMHandler<M2C_UpdateFrame>
    {
        protected override async ETVoid Run(Session session, M2C_UpdateFrame message)
        {
            var com = session.ZoneScene().GetComponent<SceneDirtyDataComponent>();
            com.Cache.Enqueue(message);
            await ETTask.CompletedTask;
        }
    }
}