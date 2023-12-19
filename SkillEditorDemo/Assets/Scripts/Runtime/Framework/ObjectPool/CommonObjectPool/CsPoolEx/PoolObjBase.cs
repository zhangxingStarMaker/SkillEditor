using System;

namespace Module.ObjectPool
{
    public abstract class PoolObjBase 
    {
        private Action<PoolObjBase> _releaseFunc = null;
        private Action<PoolObjBase> _disposeFunc = null;
        public bool HasDestroyed { set; get; } = false;
        
        protected abstract void OnReleaseToPool(); //当对象被放回池时调用
        public abstract void OnDestroy(); //当对象被销毁时调用
        public virtual void InitPool(Action<PoolObjBase> releaseFunc,Action<PoolObjBase> disposeFunc)
        {
            if (releaseFunc == null || disposeFunc == null)
            {
                PoolDebugger.LogError("releaseFunc == null || disposeFunc == null!");
                return;
            }

            _releaseFunc = releaseFunc;
            _disposeFunc = disposeFunc;
            HasDestroyed = false;
        }
        public virtual void ReleaseToPool()
        {
            OnReleaseToPool();
            _releaseFunc?.Invoke(this);
        }

        ~PoolObjBase() 
        {
            _disposeFunc?.Invoke(this);
        }
        
    }
}