namespace ET
{
    public static class FrameTimeHelper
    {
        public static int GetCurrFrame(this Entity entity)
        {
            return entity.Domain.GetComponent<SceneFrameManagerComponent>().CurrFrame;
        }

        public static int ClientFrame2ServerFrame(long clientFrame)
        {
            var n = Game.ServerFrameDuration / Game.ClientFrameDuration;
            return (int) (clientFrame + 1) / n;
        }
    }
}