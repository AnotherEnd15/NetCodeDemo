using System.Collections.Generic;

namespace ET
{
    public class UnitDiryDataComponent : Entity
    {
        public List<UpdateTransformProto> Transforms = new List<UpdateTransformProto>();
        public MoveInputProto MoveInputResultProto;
        public List<UpdateNumericProto> Numerics = new List<UpdateNumericProto>();
        public List<UnitInfo> Units = new List<UnitInfo>();
    }
}