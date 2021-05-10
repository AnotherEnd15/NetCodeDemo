using System.Collections.Generic;

namespace ET
{
    public class SceneDirtyDataComponent : Entity
    {
        public int CurrServerFrame;
        public List<UnitInfo> Units = new List<UnitInfo>();
        public List<UpdateTransformProto> Transforms = new List<UpdateTransformProto>();
        public MoveInputProto MoveInputResult;
        public long MyUnitId;
        public List<long> RemoveUnits = new List<long>();
    }
}