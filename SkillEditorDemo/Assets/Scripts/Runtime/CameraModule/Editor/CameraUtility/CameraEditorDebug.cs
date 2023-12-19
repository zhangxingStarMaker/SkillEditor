namespace CameraModule.Editor
{
    public class CameraEditorDebug
    {
        public static void LogInfo(object message)
        {
            UnityEngine.Debug.Log(message);
        }

        public static void LogWarring(object message)
        {
            UnityEngine.Debug.LogWarning(message);
        }

        public static void LogError(object message)
        {
            UnityEngine.Debug.LogError(message);
        }
    }
}