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
            return (int) (clientFrame * Game.ClientFrameDuration / Game.ServerFrameDuration);
        }
    }
}