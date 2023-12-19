using System;

namespace Module.ObjectPool
{
    public interface IActionObjectPool
    {
        void Get();
        void Release();
        void Destroy();
    }

    public class ActionObjectPool<T> where T : class, IActionObjectPool
    {
        private static readonly ObjectPool<T> ObjPool =
            new ObjectPool<T>(Create, ActionGet, ActionRelease, ActionDestory);

        private static Func<T> _createFunc = null;
        public static void Init(Func<T> createFunc)
        {
            _createFunc = createFunc;
        }
        
        private static T Create()
        {
#if DEBUG
            PoolChecker.Create(typeof(T), typeof(ActionObjectPool<T>));
#endif
            if (_createFunc == null)
            {
                PoolDebugger.LogError(string.Format("ActionObjectPool 通过反射创建了一个<color=red>'{0}'</color>类实例，这样会有损性能，请在使用前，ActionObjectPool<T>.Init()初始化对象池!!!", typeof(T)));
                return null;
            }
            return _createFunc.Invoke();
        }

        public static T Get()
        {
            return ObjPool.Get();
        }

        public static void Release(T toRelease)
        {
            if (toRelease == null)
            {
                PoolDebugger.LogError("Release Object is null");
                return;
            }
#if DEBUG
            PoolChecker.Release(toRelease.GetType(), typeof(ActionObjectPool<T>));
#endif
#if WINDOWS && DEBUG
            if (typeof(T) != toRelease.GetType())
            {
                ObjectPoolDebug.LogError("MActionObjectPool release type error, tmpType={0}, realType={1}"
                    , typeof(T).Name, toRelease.GetType().Name);
                return;
            }
#endif
            ObjPool.Release(toRelease);
        }

        public static void Clear()
        {
            ObjPool.Clear();
        }

        private static void ActionGet(T obj)
        {
            obj.Get();
        }

        private static void ActionRelease(T obj)
        {
            obj.Release();
        }

        private static void ActionDestory(T obj)
        {
            obj.Destroy();
        }
    }
}