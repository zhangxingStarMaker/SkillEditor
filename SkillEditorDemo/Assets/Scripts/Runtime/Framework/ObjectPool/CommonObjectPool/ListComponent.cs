using System;
using System.Collections.Generic;

namespace Module.ObjectPool
{
    public class ListComponent<T>: List<T>, IDisposable
    {
        private static bool _isInited = false;
        
        public static ListComponent<T> Create()
        {
            if (!_isInited)
            {
                CommonObjectPool<ListComponent<T>>.Init(CreateInstance);
            }
            return CommonObjectPool<ListComponent<T>>.Get();
        }

        private static ListComponent<T> CreateInstance()
        {
            return new ListComponent<T>();
        }

        public void Dispose()
        {
            this.Clear();
            CommonObjectPool<ListComponent<T>>.Release(this);
        }
    }
}