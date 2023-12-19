using System.Collections.Generic;
using UnityEngine;

namespace Module.ObjectPool
{
    //享元池
    public class FlyweightPool<T> where T : class
    {
        private static Dictionary<T, int> _refCountDic = new Dictionary<T, int>();

        public static void AddRef(T instance)
        {
            if (instance == null)
            {
                PoolDebugger.LogError($"FlyweightPool <{typeof(T)}> AddRef error:instance == null!");
                return;
            }
            if(!_refCountDic.TryGetValue(instance,out var refCount))
            {
                _refCountDic[instance] = 1;
            }
            else
            {
                _refCountDic[instance] = ++refCount;
            }
        }
        
        public static int ReleaseRef(T instance)
        {
            if (instance == null)
            {
                PoolDebugger.LogError($"FlyweightPool <{typeof(T)}> ReleaseRef error:instance == null!");
                return -1;
            }
            int refCount = 0;
            _refCountDic.TryGetValue(instance, out refCount);
            refCount--;
            if (refCount < 1)
            {
                _refCountDic.Remove(instance);
                if (refCount < 0)
                {
                    PoolDebugger.LogError($"FlyweightPool <{typeof(T)}> ReleaseRef error:refCount < 1!");
                }
            }
            
            return refCount;
        }
        
        
    }
}