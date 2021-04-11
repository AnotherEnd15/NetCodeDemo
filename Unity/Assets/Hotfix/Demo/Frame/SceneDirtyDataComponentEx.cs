namespace ET
{
    public static class SceneDirtyDataComponentEx
    {
        public static void Clear(this SceneDirtyDataComponent self)
        {
            self.MoveInputResult = null;
            self.Transforms.Clear();
            self.Units.Clear();
            
        }
    }
}