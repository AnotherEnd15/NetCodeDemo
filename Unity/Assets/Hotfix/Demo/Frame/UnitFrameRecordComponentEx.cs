using UnityEngine;

namespace ET
{
    public static class UnitFrameRecordComponentEx
    {
        public static void AddRecord(this UnitFrameRecordComponent self,int frame, Vector3 pos, Vector3 dir)
        {
            var frameRecord = EntityFactory.CreateWithParent<UnitFrame>(self);
            frameRecord.Pos = pos;
            frameRecord.Dir = dir;
            self.AllFrames[frame] = frameRecord;
        }
    }
}