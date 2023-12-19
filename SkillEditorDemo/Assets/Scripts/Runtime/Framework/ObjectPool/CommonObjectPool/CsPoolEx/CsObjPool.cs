using System.Collections.Generic;
using UnityEngine.Events;

namespace Module.ObjectPool
{
    public interface ICsObjectPool
    {
        int CountAll { get; }
        int CountActive { get; }
        int CountInactive { get; }
    }
    
    public class CsObjPool<T> : ICsObjectPool where T:PoolObjBase
    {
        // public
        public delegate T CreateObj();
    
        // private
        private readonly Stack<T> _stack = new Stack<T>();
        private readonly UnityAction<T> _actionOnGet;//当从池中获取一个对象调用
        private readonly UnityAction<T> _actionOnRelease;//当把对象释放入池时调用
        private readonly UnityAction<T> _actionOnDestroy;//当对象被销毁时调用(销毁后的对象不会在触发析构调用)
        private readonly UnityAction<T> _actionOnDispose;//当对象析构时调用
        private readonly CreateObj _objCreater;
        
    #if UNITY_EDITOR
            private HashSet<T> _set = new HashSet<T>();
    #endif
    
        public int CountAll { get; private set; }
        public int CountActive => CountAll - CountInactive;
        public int CountInactive => _stack.Count;
        private int _maxCapacity = -1;
        public CsObjPool(CreateObj creater, UnityAction<T> actionOnGet, UnityAction<T> actionOnRelease,
            UnityAction<T> actionOnDestroy = null,int maxCapacity = -1, UnityAction<T> actionOnDispose=null)
        {
            _objCreater = creater;
            _actionOnGet = actionOnGet;
            _actionOnRelease = actionOnRelease;
            _actionOnDestroy = actionOnDestroy;
            _actionOnDispose = actionOnDispose;
            _maxCapacity = maxCapacity;
        }
    
        public T Get()
        {
            T element;
            if (_stack.Count == 0)
            {
                element = _objCreater();
                
                element.InitPool(ReleaseObj,DisposeObj);
                CountAll++;
            }
            else
            {
                element = _stack.Pop();
                element.InitPool(ReleaseObj,DisposeObj);
    #if UNITY_EDITOR
                    _set.Remove(element);
    #endif
            }
    
            _actionOnGet?.Invoke(element);
    
            return element;
        }

        private void ReleaseObj(PoolObjBase obj)
        {
            Release(obj as T);
        }
        
        private void DisposeObj(PoolObjBase obj)
        {
            NotifyPoolObjDispose(obj as T);
        }
        
        public void Release(T element)
        {
    #if UNITY_EDITOR
                if (_set.Contains(element))
                {
                    PoolDebugger.LogError(element.GetType().ToString() + " release twice!");
                    return;
                }
                _set.Add(element);
    #endif
            
            _actionOnRelease?.Invoke(element);
            if (_maxCapacity <= 0 || _maxCapacity>_stack.Count)
            {
                _actionOnRelease?.Invoke(element);
                _stack.Push(element);
            }
            else
            {
                DestroyElement(element);
            }
        }

        private void DestroyElement(T element)
        {
            if (element == null)
            {
                return;
            }
            element.HasDestroyed = true;
            _actionOnDestroy?.Invoke(element);
        }
        
        public void Clear()
        {
            while (_stack.Count > 0)
                DestroyElement(_stack.Pop());
            
            _stack.Clear();
        }
        
        public void ForceReleaseUnUsedObj(uint remainObjNum)
        {
            while (_stack.Count>remainObjNum)
            {
                var poolObj = _stack.Pop();
                _actionOnDestroy?.Invoke(poolObj);
            }
        }

        public void NotifyPoolObjDispose(T element)
        {
            if (element == null || element.HasDestroyed || _actionOnDispose==null)
            {
                return;
            }
            _actionOnDispose.Invoke(element);
        }
    }
}