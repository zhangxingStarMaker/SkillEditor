using System.Collections.Generic;

namespace Module.ObjectPool
{
    public class DictionaryPool<K, V>
    {
        // Object pool to avoid allocations.
        private static readonly ObjectPool<Dictionary<K, V>> s_pool =
            new ObjectPool<Dictionary<K, V>>(Create, l => l.Clear(), l => l.Clear());

        private static Dictionary<K, V> Create()
        {
            return new Dictionary<K, V>();
        }

        public static Dictionary<K, V> Get()
        {
            return s_pool.Get();
        }

        public static void Release(Dictionary<K, V> toRelease)
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
