using System;
using System.Collections.Generic;
using Moudule.ETTaskCore;
using Runtime.Framework.FrameUtility;

namespace Module.FrameBase
{
    public class GameSingletonCenter
    {
        private readonly Dictionary<Type, ISingleton> _singletonTypes = new Dictionary<Type, ISingleton>();
        private readonly Stack<ISingleton> _singletons = new Stack<ISingleton>();
        private readonly Queue<ISingleton> _onGUIs = new Queue<ISingleton>();
        private readonly Queue<ISingleton> _updates = new Queue<ISingleton>();
        private readonly Queue<ISingleton> _lateUpdates = new Queue<ISingleton>();
        private readonly Queue<ETTask> _frameFinishTask = new Queue<ETTask>();
        private uint _updateTick = 0;
        
        public T AddSingleton<T>() where T: Singleton<T>, new()
        {
            T singleton = new T();
            AddSingleton(singleton);
            return singleton;
        }
        
        public T AddSingleton<T,P>(P awakeParam) where T: Singleton<T>, new()
        {
            T singleton = new T();
            AddSingleton(singleton, awakeParam);
            return singleton;
        }
        
        private void AddSingleton(ISingleton singleton)
        {
            Type singletonType = singleton.GetType();
            if (_singletonTypes.ContainsKey(singletonType))
            {
                throw new Exception($"already exist singleton: {singletonType.Name}");
            }

            _singletonTypes.Add(singletonType, singleton);
            _singletons.Push(singleton);
            
            singleton.Register();

            if (singleton is ISingletonAwake awake)
            {
                awake.Awake();
            }

            if (singleton is ISingletonOnGUI)
            {
                _onGUIs.Enqueue(singleton);
            }
            
            if (singleton is ISingletonUpdate)
            {
                _updates.Enqueue(singleton);
            }
            
            if (singleton is ISingletonLateUpdate)
            {
                _lateUpdates.Enqueue(singleton);
            }
        }

        private void AddSingleton<TP>(ISingleton singleton,TP awakeParam)
        {
            Type singletonType = singleton.GetType();
            if (_singletonTypes.ContainsKey(singletonType))
            {
                throw new Exception($"already exist singleton: {singletonType.Name}");
            }

            _singletonTypes.Add(singletonType, singleton);
            _singletons.Push(singleton);
            
            singleton.Register();

            if (singleton is ISingletonAwake<TP> awake)
            {
                awake.Awake(awakeParam);
            }

            if (singleton is ISingletonOnGUI)
            {
                _onGUIs.Enqueue(singleton);
            }
            
            if (singleton is ISingletonUpdate)
            {
                _updates.Enqueue(singleton);
            }
            
            if (singleton is ISingletonLateUpdate)
            {
                _lateUpdates.Enqueue(singleton);
            }
        }
        
        public async ETTask WaitFrameFinish()
        {
            ETTask task = ETTask.Create(true);
            _frameFinishTask.Enqueue(task);
            await task;
        }

        public void NotifyAllSingletonCreateComplete()
        {
            foreach (var tempSingle in _singletons)
            {
                if (tempSingle is IAllSingletonCreateComplete obj)
                {
                    obj.OnAllSingletonCreateComplete();
                }
            }
        }
        
        public void NotifyAllSingletonGameStart()
        {
            foreach (var tempSingle in _singletons)
            {
                if (tempSingle is ISingletonNotifyGameStart obj)
                {
                    obj.OnGameStart();
                }
            }
        }
        
        public void OnGUI()
        {
            int count = _onGUIs.Count;
            while (count-- > 0)
            {
                ISingleton singleton = _onGUIs.Dequeue();

                if (singleton.IsDisposed())
                {
                    continue;
                }

                if (singleton is not ISingletonOnGUI tempOnGUI)
                {
                    continue;
                }
                
                _onGUIs.Enqueue(singleton);
                try
                {
                    tempOnGUI.OnGUI();
                }
                catch (Exception e)
                {
                    FrameworkLog.LogError(e.ToString());
                }
            }
        }
        
        public void Update()
        {
            _updateTick++;
            int count = _updates.Count;
            while (count-- > 0)
            {
                ISingleton singleton = _updates.Dequeue();

                if (singleton.IsDisposed())
                {
                    continue;
                }

                if (singleton is not ISingletonUpdate update)
                {
                    continue;
                }
                
                _updates.Enqueue(singleton);
                try
                {
                    update.Update(_updateTick);
                }
                catch (Exception e)
                {
                    FrameworkLog.LogError(e.ToString());
                }
            }
        }
        
        public void LateUpdate()
        {
            int count = _lateUpdates.Count;
            while (count-- > 0)
            {
                ISingleton singleton = _lateUpdates.Dequeue();
                
                if (singleton.IsDisposed())
                {
                    continue;
                }

                if (singleton is not ISingletonLateUpdate lateUpdate)
                {
                    continue;
                }
                
                _lateUpdates.Enqueue(singleton);
                try
                {
                    lateUpdate.LateUpdate(_updateTick);
                }
                catch (Exception e)
                {
                    FrameworkLog.LogError(e.ToString());
                }
            }
        }

        public void FrameFinishUpdate()
        {
            while (_frameFinishTask.Count > 0)
            {
                ETTask task = _frameFinishTask.Dequeue();
                task.SetResult();
            }
        }

        public void Close()
        {
            _updateTick = 0;
            // 顺序反过来清理
            while (_singletons.Count > 0)
            {
                ISingleton iSingleton = _singletons.Pop();
                iSingleton.Destroy();
            }
            _singletonTypes.Clear();
        }
    }
}