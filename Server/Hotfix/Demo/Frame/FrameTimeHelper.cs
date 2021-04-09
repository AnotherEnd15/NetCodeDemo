namespace ET
{
    public static class FrameTimeHelper
    {
        public static int GetCurrFrame(this Entity entity)
        {
            return entity.Domain.GetComponent<SceneFrameManagerComponent>().CurrFrame;
        }
    }
}