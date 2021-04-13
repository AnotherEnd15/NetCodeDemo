using UnityEngine;

namespace ET
{
    public static class TransformUpdateComponentEx
    {
        public static void SetPosTarget(this TransformUpdateComponent self, Vector3 target)
        {
            self.TargetPos = target;
        }

        public static void SetDirTarget(this TransformUpdateComponent self, Vector3 target)
        {
            self.TargetDir = target;
        }
    }
}