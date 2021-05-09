namespace ET
{
    public static class FrameTimeHelper
    {
        public static int GetCurrSimulateFrame(this Entity entity)
        {
            return entity.Domain.GetComponent<SceneFrameManagerComponent>().CurrSimulateFrame;
        }
        
        public static int GetLastServerFrame(this Entity entity)
        {
            return entity.Domain.GetComponent<SceneFrameManagerComponent>().LastServerFrame;
        }

        public static void SetServerFrame(Entity entity,int serverFrame)
        {
            var com = entity.Domain.GetComponent<SceneFrameManagerComponent>();
            com.LastServerFrame = serverFrame;
            // 计算模拟的开始帧
            var pingCom = entity.CurrSession().GetComponent<PingComponent>();
            var deltaFrame = pingCom.Ping / Game.ClientFrameDuration + 1;
            var minSimulateFrame = com.LastServerFrame + (int)deltaFrame;
            if (com.CurrSimulateFrame < minSimulateFrame)
                com.CurrSimulateFrame = minSimulateFrame;
        }
    }
}