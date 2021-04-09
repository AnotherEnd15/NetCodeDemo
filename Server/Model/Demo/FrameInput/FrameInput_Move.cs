using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class FrameInput_Move : Entity
    {
        public Vector3 Target;
        public List<Vector3> Path = new List<Vector3>();
    }
}