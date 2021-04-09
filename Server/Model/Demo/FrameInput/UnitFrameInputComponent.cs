namespace ET
{
    public class UnitFrameInputComponent : Entity
    {
        // Childern存储帧输入,没收到这一帧输入的时候,每帧开始就模拟最后一次的输入
        public FrameInput LastInput;
    }
}