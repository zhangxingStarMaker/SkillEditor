 using System.Collections.Generic;

 namespace Module.ObjectPool
 {
     public class QueuePool<T>
     {
         // Object pool to avoid allocations.
         private static readonly ObjectPool<Queue<T>> s_pool =
             new ObjectPool<Queue<T>>(Create, l => l.Clear(), l => l.Clear());

         private static Queue<T> Create()
         {
             return new Queue<T>();
         }

         public static Queue<T> Get()
         {
             return s_pool.Get();
         }

         public static void Release(Queue<T> toRelease)
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
