namespace ET
{
    public static class FrameTimeHelper
    {
        public static int GetCurrSimulateFrame(this Entity entity)
        {
            return entity.Domain.GetComponent<SceneFrameManagerComponent>().CurrSimulateFrame;
        }
        
        public static int GetSimulateServerFrame(this Entity entity)
        {
            return entity.Domain.GetComponent<SceneFrameManagerComponent>().SimulateServerFrame;
        }

        public static void SetServerFrame(Entity entity,int serverFrame)
        {
            var com = entity.Domain.GetComponent<SceneFrameManagerComponent>();
            if (com.LastServerFrame == 0)
            {
                com.LastServerFrame = serverFrame;
                // 计算模拟的开始帧
                var pingCom = entity.CurrSession().GetComponent<PingComponent>();
                var deltaFrame = (pingCom.RTT + Game.ServerFrameDuration) / Game.ClientFrameDuration;
                Log.Debug($"当前多模拟帧数 {deltaFrame} RTT {pingCom.RTT}");
                var minSimulateFrame = com.LastServerFrame + (int)deltaFrame;
                com.CurrSimulateFrame = minSimulateFrame;
            }
            else
            {
                com.LastServerFrame = serverFrame;
                if (com.CurrSimulateFrame < com.LastServerFrame)
                    com.CurrSimulateFrame = com.LastServerFrame;
            }

          
        }
    }
}