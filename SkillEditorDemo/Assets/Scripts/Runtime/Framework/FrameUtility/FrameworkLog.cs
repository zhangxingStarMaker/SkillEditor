namespace Runtime.Framework.FrameUtility
{
    public class FrameworkLog
    {
        public static void LogInfo(string log)
        {
            UnityEngine.Debug.Log(log);
        }
        
        public static void LogWarning(string log)
        {
            UnityEngine.Debug.LogWarning(log);
        }
        
        public static void LogError(string log)
        {
            UnityEngine.Debug.LogError(log);
        }
    }
}