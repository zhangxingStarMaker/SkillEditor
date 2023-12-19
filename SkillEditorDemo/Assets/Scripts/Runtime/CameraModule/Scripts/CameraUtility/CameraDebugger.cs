using Runtime.Framework.FrameUtility;

namespace CameraModule.Runtime
{
    public static class CameraDebugger
    {
        public static void LogError(string log)
        {
            FrameworkLog.LogError(log);
        }

        public static void LogWarning(string log)
        {
            FrameworkLog.LogWarning(log);
        }

        public static void Log(string log)
        {
            FrameworkLog.LogInfo(log);
        }
    }
}