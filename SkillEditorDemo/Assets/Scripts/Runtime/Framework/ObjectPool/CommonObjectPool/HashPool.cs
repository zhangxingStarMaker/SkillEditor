using System.Collections.Generic;

namespace Module.ObjectPool
{
    public class HashPool<T>
    {
        // Object pool to avoid allocations.
        private static readonly ObjectPool<HashSet<T>> SPool =
            new ObjectPool<HashSet<T>>(Create, l => l.Clear(), l => l.Clear());

        private static HashSet<T> Create()
        {
            return new HashSet<T>();
        }

        public static HashSet<T> Get()
        {
            return SPool.Get();
        }

        public static void Release(HashSet<T> toRelease)
        {
#if DEBUG
            if (toRelease == null) PoolDebugger.LogError("toRelease is null");
#endif
            SPool.Release(toRelease);
        }

        public static void Clear()
        {
            SPool.Clear();
        }
    }
}
