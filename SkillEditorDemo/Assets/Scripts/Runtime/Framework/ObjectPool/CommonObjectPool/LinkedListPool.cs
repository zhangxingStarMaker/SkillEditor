using System.Collections.Generic;

namespace Module.ObjectPool
{
    public class LinkedListPool<T>
    {
        // Object pool to avoid allocations.
        private static readonly ObjectPool<LinkedList<T>> s_pool =
            new ObjectPool<LinkedList<T>>(Create, l => l.Clear(), l => l.Clear());

        private static LinkedList<T> Create()
        {
            return new LinkedList<T>();
        }

        public static LinkedList<T> Get()
        {
            return s_pool.Get();
        }

        public static void Release(LinkedList<T> toRelease)
        {
#if DEBUG
            if (toRelease == null) PoolDebugger.LogError("toRelease is null");
#endif
            s_pool.Release(toRelease);
        }

        public static void Clear()
        {
            s_pool.Clear();
        }
    }
}
