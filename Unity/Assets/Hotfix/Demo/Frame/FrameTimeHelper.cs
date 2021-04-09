namespace ET
{
    public static class FrameTimeHelper
    {
        public static int GetCurrSimulateFrame(this Entity entity)
        {
            return entity.Domain.GetComponent<SceneFrameManagerComponent>().CurrSimulateFrame;
        }
    }
}