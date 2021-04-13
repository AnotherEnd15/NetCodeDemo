using MongoDB.Bson.Serialization.Conventions;

namespace ET
{
    public static class SceneHelper
    {
        public static int DomainZone(this Entity entity)
        {
            return ((Scene) entity.Domain)?.Zone ?? 0;
        }

        public static Scene DomainScene(this Entity entity)
        {
            var scene = (Scene) entity.Domain;
            return scene;
        }
    }
}