namespace ET
{
    public class PingComponent: Entity
    {
        [NoMemoryCheck]
        public C2G_Ping C2G_Ping = new C2G_Ping();

        public long RTT; //延迟值
    }
}