using System;

namespace Module.ObjectPool
{

    public interface IClearObj
    {
        void Clear();
    }

    public class CommonObjectPool<T> where T : class
    {
        // Object pool to avoid allocations.
        private static readonly ObjectPool<T> SPool = new ObjectPool<T>(Create, null, null);
        private static Func<T> _createInstanceFunc = null;

        private static T Create()
        {
#if DEBUG
            PoolChecker.Create(typeof(T), typeof(CommonObjectPool<T>));
#endif
            if (_createInstanceFunc == null)
            {
                PoolDebugger.LogError(string.Format(
                    "CommonObjectPool 通过反射创建了一个<color=red>'{0}'</color>类实例，这样会有损性能，请在使用前，用CommonObjectPool<T>.Init()初始化对象池!!!",
                    typeof(T)));
                return null;
            }

            return _createInstanceFunc();
        }


        public static void Init(Func<T> createInstanceFunc)
        {
            _createInstanceFunc = createInstanceFunc;
        }

        public static T Get()
        {
            var t = SPool.Get();
#if DEBUG
            if (t is IActionObjectPool)
                PoolDebugger.LogError(string.Format("{0} has implement IActionObjectPool!!!", t.GetType()));
#endif

            if (t is IClearObj)
            {
                IClearObj clearObj = t as IClearObj;
                clearObj.Clear();
            }

            return t;
        }

        public static void Release(T toRelease)
        {
            if (toRelease == null)
            {
                PoolDebugger.LogError("Release Object is null");
                return;
            }

#if DEBUG
            PoolChecker.Release(toRelease.GetType(), typeof(CommonObjectPool<T>));
#endif
#if UNITY_EDITOR && DEBUG
            if (typeof(T) != toRelease.GetType())
            {
                PoolDebugger.LogError(string.Format(
                    "MActionObjectPool release type error, tmpType={0}, realType={1}"
                    , typeof(T).Name, toRelease.GetType().Name));
                return;
            }
#endif
            if (toRelease is IClearObj)
            {
                IClearObj clearObj = toRelease as IClearObj;
                clearObj.Clear();
            }

            SPool.Release(toRelease);
        }

        public static void Clear()
        {
            SPool.Clear();
        }
    }
}
