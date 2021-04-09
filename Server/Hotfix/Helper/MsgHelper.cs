using UnityEngine;

namespace ET
{
    public static class MsgHelper
    {
        public static OpVector3 ToOpV3(this Vector3 v)
        {
            return new OpVector3() { X = v.x, Y = v.y, Z = v.z };
        }
        
        public static Vector3 ToV3(this OpVector3 v)
        {
            return new Vector3() { x = v.X, y = v.Y, z = v.Z };
        }
    }
}