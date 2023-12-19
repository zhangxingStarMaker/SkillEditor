using System;
using System.Collections.Generic;

namespace Module.ObjectPool
{
    public class HashSetPool<T>: HashSet<T>, IDisposable
    {
        private static bool _isInited = false;
        public static HashSet<T> Create()
        {
            if (!_isInited)
            {
                CommonObjectPool<HashSet<T>>.Init(CreateInstance);
                _isInited = true;
            }
            
            return CommonObjectPool<HashSet<T>>.Get();;
        }

        public static void Release(HashSet<T> obj)
        {
#if DEBUG
            if (obj == null) PoolDebugger.LogError("Release error: obj is null!");
#endif
            CommonObjectPool<HashSet<T>>.Release(obj);
        }
        
        static HashSet<T> CreateInstance()
        {
            return new HashSet<T>();
        }

        public void Dispose()
        {
            Clear();
            CommonObjectPool<HashSetPool<T>>.Release(this);
        }
    }
}