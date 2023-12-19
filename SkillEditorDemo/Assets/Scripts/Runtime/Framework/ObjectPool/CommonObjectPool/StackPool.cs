using System.Collections.Generic;

namespace Module.ObjectPool
{
    public class StackPool<T>
    {
        // Object pool to avoid allocations.
        private static readonly ObjectPool<Stack<T>> SPool =
            new ObjectPool<Stack<T>>(Create, l => l.Clear(), l => l.Clear());

        private static Stack<T> Create()
        {
            return new Stack<T>();
        }

        public static Stack<T> Get()
        {
            return SPool.Get();
        }

        public static void Release(Stack<T> toRelease)
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
