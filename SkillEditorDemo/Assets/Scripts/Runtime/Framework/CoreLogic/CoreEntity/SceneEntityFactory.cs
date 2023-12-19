using Runtime.Framework.FrameUtility;

namespace Module.FrameBase
{
    public static class SceneEntityFactory
    {
        public static LogicScene CreateLogicScene(long id, long instanceId, int zone, SceneType sceneType, string name, CoreEntity parent = null)
        {
            LogicScene logicScene = new LogicScene(id, instanceId, zone, sceneType, name, parent);
            return logicScene;
        }

        public static LogicScene CreateLogicScene(int zone, SceneType sceneType, string name, CoreEntity parent = null)
        {
            long instanceId = IdGenerator.Instance.GenerateInstanceId();
            LogicScene logicScene = new LogicScene(zone, instanceId, zone, sceneType, name, parent);
            return logicScene;
        }
    }
}