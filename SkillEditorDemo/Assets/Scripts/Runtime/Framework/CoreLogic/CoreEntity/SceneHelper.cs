namespace Module.FrameBase
{
    public static class SceneHelper
    {
        public static int DomainZone(this CoreEntity coreEntity)
        {
            return ((LogicScene) coreEntity.Domain)?.Zone ?? 0;
        }

        public static LogicScene DomainScene(this CoreEntity coreEntity)
        {
            return (LogicScene) coreEntity.Domain;
        }
    }
}