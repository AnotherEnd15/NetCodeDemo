using System.Collections.Generic;

namespace ET
{
    public class UnitFrameInputComponent : Entity
    {
        public CircularList<FrameInput> AllInputs = new CircularList<FrameInput>(20);
    }
}