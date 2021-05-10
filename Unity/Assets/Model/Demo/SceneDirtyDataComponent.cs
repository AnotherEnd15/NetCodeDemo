using System.Collections.Generic;

namespace ET
{
    public class SceneDirtyDataComponent : Entity
    {
        public Queue<M2C_UpdateFrame> Cache = new Queue<M2C_UpdateFrame>();
    }
}