using System.Collections.Generic;

namespace ET
{
    public class SceneDirtyDataComponent : Entity
    {
        public List<UnitInfo> Units = new List<UnitInfo>();
        public List<UpdateTransformProto> Transforms = new List<UpdateTransformProto>();
        public MoveInputProto MoveInputResult = new MoveInputProto();
        public long MyUnitId;
    }
}