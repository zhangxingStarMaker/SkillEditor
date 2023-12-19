namespace Module.ObjectPool
{
    public interface IPoolDebugger
    {
        void LogError(string log);
        void LogWarning(string log);
        void LogInfo(string log);
    }
    public static class PoolDebugger
    {
        private static IPoolDebugger _debugger = null;
        
        public static void RegisterDebugger(IPoolDebugger debugger)
        {
            _debugger = debugger;
        }
        
        public static void LogError(string log)
        {
            _debugger?.LogError(log);
        }

        public static void LogWarning(string log)
        {
            _debugger?.LogWarning(log);
        }

        public static void Log(string log)
        {
            _debugger?.LogInfo(log);
        }
    }
}