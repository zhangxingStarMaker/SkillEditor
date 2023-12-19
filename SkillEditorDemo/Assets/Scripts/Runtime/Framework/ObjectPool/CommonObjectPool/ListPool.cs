using System.Collections.Generic;

namespace Module.ObjectPool
{
    public class ListPool<T>
    {
        // Object pool to avoid allocations.
        private static readonly ObjectPool<List<T>> SPool =
            new ObjectPool<List<T>>(Create, l => l.Clear(), l => l.Clear());

        private static List<T> Create()
        {
            return new List<T>();
        }

        public static List<T> Get()
        {
            return SPool.Get();
        }

        public static void Release(List<T> toRelease)
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