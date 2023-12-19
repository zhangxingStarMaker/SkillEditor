using System;
using System.Collections.Generic;
using UnityEngine.Events;
namespace Module.ObjectPool
{
/// <summary>
///     从UGUI源码中挪过来的
/// </summary>
/// <typeparam name="T"></typeparam>
public class ObjectPool<T> : IObjectPool
{
    // public
    public delegate T CreateObj();

    // private
    private readonly Stack<T> _stack = new Stack<T>();
    private readonly UnityAction<T> _actionOnGet;
    private readonly UnityAction<T> _actionOnRelease;
    private readonly UnityAction<T> _actionOnDestroy;
    private readonly CreateObj _objCreater;

#if UNITY_EDITOR && DEBUG
        private HashSet<T> _set = new HashSet<T>();
#endif

    public int CountAll { get; private set; }
    public int CountActive => CountAll - CountInactive;
    public int CountInactive => _stack.Count;

    public ObjectPool(CreateObj creater, UnityAction<T> actionOnGet, UnityAction<T> actionOnRelease,
        UnityAction<T> actionOnDestroy = null)
    {
        _objCreater = creater;
        _actionOnGet = actionOnGet;
        _actionOnRelease = actionOnRelease;
        _actionOnDestroy = actionOnDestroy;
    }

    private int count = 0;
    public T Get()
    {
        T element;
        if (_stack.Count == 0)
        {
            element = _objCreater();
            CountAll++;
        }
        else
        {
            element = _stack.Pop();
#if UNITY_EDITOR && DEBUG
                _set.Remove(element);
#endif
        }

        _actionOnGet?.Invoke(element);

        return element;
    }

    public void Release(T element)
    {
#if UNITY_EDITOR && DEBUG
            if (_set.Contains(element))
            {
                PoolDebugger.LogError(element.GetType().ToString() + " release twice!");
                return;
            }
            _set.Add(element);
#endif

        _actionOnRelease?.Invoke(element);

        _stack.Push(element);
    }

    public void Clear()
    {
        if (_actionOnDestroy != null)
            while (_stack.Count > 0)
                _actionOnDestroy(_stack.Pop());
        _stack.Clear();
    }
}

#if DEBUG
public class PoolChecker
{
    private static readonly Dictionary<Type, Type> _objToCreater = new Dictionary<Type, Type>();

    public static void Create(Type obj, Type curCreater)
    {
        if (_objToCreater.TryGetValue(obj, out var preCreater))
        {
            if (!IsSameType(preCreater, curCreater))
                PoolDebugger.LogError($"kicode Create obj:{obj} by preCreater:{preCreater}, curCreater:{curCreater}");
        }
        else
        {
            if (obj.FullName.Contains(@"System.Collections.Generic"))
                PoolDebugger.LogError($"please use others pool create:{obj.FullName}!!!");
            _objToCreater.Add(obj, curCreater);
        }
    }

    private static bool IsSameType(Type a, Type b)
    {
        return a == b;
    }

    public static void Release(Type obj, Type curCreater)
    {
        if (_objToCreater.TryGetValue(obj, out var preCreater))
        {
            if (!IsSameType(preCreater, curCreater))
                PoolDebugger.LogError($"kicode Release obj:{obj} by preCreater:{preCreater}, curCreater:{curCreater}");
        }
        else
        {
            PoolDebugger.LogError($"kicode Release obj:{obj} by preCreater is null, curCreater:{curCreater}");
        }
    }
}
#endif
}


