using UnityEngine;

namespace Moudule.ETTaskCore
{
    public interface IEtTaskDebugger
    {
        void LogError(string log);
        void LogWarning(string log);
        void LogInfo(string log);
        void DebugError(string text, string tag = null, string color = "red");
    }
    
    public static class EtTaskDebugger
    {
        private static IEtTaskDebugger _debugger = null;
        
        public static void RegisterDebugger(IEtTaskDebugger debugger)
        {
            _debugger = debugger;
        }
        
        public static void LogError(string log)
        {
            if (_debugger == null)
            {
                Debug.LogError(log);
                return;
            }
            _debugger.LogError(log);
        }

        public static void DebugError(string text, string tag = null, string color = "red")
        {
            if (_debugger == null)
            {
                Debug.LogError(text);
                return;
            }
            _debugger.DebugError(text,tag,color);
        }

        public static void LogWarning(string log)
        {
            if (_debugger == null)
            {
                Debug.LogError(log);
                return;
            }
            _debugger.LogWarning(log);
        }

        public static void Log(string log)
        {
            if (_debugger == null)
            {
                Debug.LogError(log);
                return;
            }
            _debugger.LogInfo(log);
        }
    }
}