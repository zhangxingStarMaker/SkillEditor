using System;
using System.Collections.Generic;
using Module.Utility;
using Moudule.ETTaskCore;
using Runtime.Framework.FrameUtility;

namespace Module.FrameBase
{
    public class CoreEventSystem:Singleton<CoreEventSystem>, ISingletonAwake,ISingletonUpdate, ISingletonLateUpdate,ISingletonOnGUI
    {
        private readonly UnOrderMultiMapSet<Type, Type> types = new();
        private readonly Dictionary<CoreEntityExecuteQueueIndex,List<ExecuteQueueInfo>> queueEntityDic =
            new Dictionary<CoreEntityExecuteQueueIndex, List<ExecuteQueueInfo>>();
       
        private readonly EventDispatcher _globalEventDispatcher = new EventDispatcher();
        
        #region 获取类信息
        
        public HashSet<Type> GetTypes(Type systemAttributeType)
        {
            if (!types.ContainsKey(systemAttributeType))
            {
                return new HashSet<Type>();
            }

            return types[systemAttributeType];
        }
        
        #endregion

        #region 更新队列信息
        public void NotifyAddNewEntity(CoreEntity newCoreEntity)
        {
            //创建新实体后，更新生命周期队列
            EntityExecuteOrder tempOrder = EntityExecuteOrder.Normal;
            if (newCoreEntity is ICoreEntitySort tempEntitySort)
            {
                tempOrder = tempEntitySort.GetSortId();
            }
            if (newCoreEntity is ICoreEntityGUI)
            {
                AddEntityToExecuteQueue(newCoreEntity, CoreEntityExecuteQueueIndex.OnGUI, tempOrder);
            }
            if (newCoreEntity is ICoreEntityPreload)
            {
                AddEntityToExecuteQueue(newCoreEntity, CoreEntityExecuteQueueIndex.Preload, tempOrder);
            }
            if (newCoreEntity is ICoreEntityUpdate)
            {
                AddEntityToExecuteQueue(newCoreEntity, CoreEntityExecuteQueueIndex.Update, tempOrder);
            }
            if (newCoreEntity is ICoreEntityLateUpdate)
            {
                AddEntityToExecuteQueue(newCoreEntity, CoreEntityExecuteQueueIndex.LateUpdate, tempOrder);
            }
        }

        private readonly List<Queue<ExecuteQueueInfo>> _tempUseExeQueueInfoQueueList = new List<Queue<ExecuteQueueInfo>>();
        private Queue<ExecuteQueueInfo> GetTempUseExeQueueInfo()
        {
            Queue<ExecuteQueueInfo> tempExeQueueInfoQueue = null;
            var cacheCount = _tempUseExeQueueInfoQueueList.Count;
            if (cacheCount < 1)
            {
                return new Queue<ExecuteQueueInfo>();
            }
            tempExeQueueInfoQueue = _tempUseExeQueueInfoQueueList[cacheCount-1];
            _tempUseExeQueueInfoQueueList.RemoveAt(cacheCount-1);
            return tempExeQueueInfoQueue;
        }

        private void ReleaseTempUseExeQueueInfo(Queue<ExecuteQueueInfo> exeQueueInfoQueue)
        {
            if (exeQueueInfoQueue == null)
            {
                return;
            }
            if (exeQueueInfoQueue.Count > 0)
            {
                FrameworkLog.LogError($"ReleaseTempUseExeQueueInfo error:exeQueueInfoQueue.Count > 0!");
                return;
            }
            _tempUseExeQueueInfoQueueList.Add(exeQueueInfoQueue);
        }
        
        private async ETTask ExecuteQueue(CoreEntityExecuteQueueIndex queueIndex,bool isASync,IExecuteQueueInfoArg arg)
        {
            if (!queueEntityDic.TryGetValue(queueIndex, out var tempExeQueueInfoList) || tempExeQueueInfoList == null)
            {
                return;
            }
            var queueInfoLen = tempExeQueueInfoList.Count;
            if (queueInfoLen < 1)
            {
                return;
            }
            var tempUseExeQueueInfoQueue = GetTempUseExeQueueInfo();
            //避免在执行的过程中插入新的ExeQueueInfo，导致当前队列重复执行，所以转入一个新的队列执行
            for (int i = 0; i < queueInfoLen; i++)
            {
                var tempExeQueueInfo = tempExeQueueInfoList[i];
                if (tempExeQueueInfo == null)
                {
                    continue;
                }
                tempUseExeQueueInfoQueue.Enqueue(tempExeQueueInfo);
            }
            for (int i = 0; i < tempUseExeQueueInfoQueue.Count; i++)
            {
                var tempExeQueueInfo = tempUseExeQueueInfoQueue.Dequeue();
                if (isASync)
                {
                    await tempExeQueueInfo.ExecuteASync(arg);
                }
                else
                {
                    tempExeQueueInfo.Execute(arg);
                }
            }
            ReleaseTempUseExeQueueInfo(tempUseExeQueueInfoQueue);
        }
        private void AddEntityToExecuteQueue(CoreEntity newCoreEntity,CoreEntityExecuteQueueIndex queueIndex,EntityExecuteOrder executeOrder)
        {
            if (!queueEntityDic.TryGetValue(queueIndex, out var tempExeQueueInfoList) || tempExeQueueInfoList == null)
            {
                tempExeQueueInfoList = new List<ExecuteQueueInfo>();
                queueEntityDic[queueIndex] = tempExeQueueInfoList;
            }
            int curQueueInfoListLen = tempExeQueueInfoList.Count;
            ExecuteQueueInfo tempQueueInfo = null;
            
            if (curQueueInfoListLen > 0)
            {
                for (int i = 0; i < tempExeQueueInfoList.Count; i++)
                {
                    var tempExeQueueInfo = tempExeQueueInfoList[i];
                    if (tempExeQueueInfo.ExecuteOrder == executeOrder)
                    {
                        tempQueueInfo = tempExeQueueInfo;
                        continue;
                    }
                    if (tempExeQueueInfo.ExecuteOrder < executeOrder)
                    {
                        tempQueueInfo = CreateExecuteQueueInfo(queueIndex);
                        tempExeQueueInfoList.Insert(i,tempQueueInfo);
                    }
                }
            }
            if (tempQueueInfo == null)
            {
                tempQueueInfo = CreateExecuteQueueInfo(queueIndex);
                tempExeQueueInfoList.Add(tempQueueInfo);
            }
            
            tempQueueInfo.AddEntity(newCoreEntity);
        }
        private ExecuteQueueInfo CreateExecuteQueueInfo(CoreEntityExecuteQueueIndex queueIndex)
        {
            ExecuteQueueInfo tempExecuteQueueInfo = null;
            if (queueIndex == CoreEntityExecuteQueueIndex.Preload)
            {
                tempExecuteQueueInfo = new ExecutePreloadQueueInfo();
            }
            else if (queueIndex == CoreEntityExecuteQueueIndex.OnGUI)
            {
                tempExecuteQueueInfo = new ExecuteGUIQueueInfo();
            }
            else if (queueIndex == CoreEntityExecuteQueueIndex.Update)
            {
                tempExecuteQueueInfo = new ExecuteUpdateQueueInfo();
            }
            else if (queueIndex == CoreEntityExecuteQueueIndex.LateUpdate)
            {
                tempExecuteQueueInfo = new ExecuteLateUpdateQueueInfo();
            }
            else
            {
                FrameworkLog.LogError($"AddEntityToExecuteQueue error, 尚未支持的QueueIndex：{queueIndex}!");
            }
            return tempExecuteQueueInfo;
        }
        #endregion

        #region lifestyle

        public void Update(uint updateTick)
        {
            ExecuteQueue(CoreEntityExecuteQueueIndex.Update,false,null);
        }

        public void LateUpdate(uint updateTick)
        {
            ExecuteQueue(CoreEntityExecuteQueueIndex.LateUpdate,false,null);
        }

        public void OnGUI()
        {
            ExecuteQueue(CoreEntityExecuteQueueIndex.OnGUI,false,null);
        }

        public void Awake()
        {
            RecordAllTypeInfo();
        }
        
        void RecordAllTypeInfo()
        {
            Dictionary<string, Type> allTypeDic = new Dictionary<string, Type>();
            var allAssemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            types.Clear();
            foreach (var tempAssembly in allAssemblies)
            {
                var allTypes = tempAssembly.GetTypes();
                for (int i = 0; i < allTypes.Length; i++)
                {
                    var tempType = allTypes[i];
                    var typeFullName = tempType.FullName;
                    if (allTypeDic.ContainsKey(typeFullName))
                    {
                        continue;
                    }
                    allTypeDic.Add(typeFullName,tempType);
                    if (tempType.IsAbstract)
                    {
                        continue;
                    }
                    // 记录所有的有BaseAttribute标记的的类型
                    object[] objects = tempType.GetCustomAttributes(typeof(BaseAttribute), true);
                    if (objects.Length < 1)
                    {
                        continue;
                    }
                    foreach (object o in objects)
                    {
                        types.Add(o.GetType(), tempType);
                    }
                }
            }
        }
        
        public async ETTask CollectPreloadInfo(IPreloadComponent preloadCom,PreloadOpportunity opportunity)
        {
            if (preloadCom == null)
            {
                FrameworkLog.LogError($"CollectPreloadInfo error:preloadCom == null,{opportunity}!");
                return;
            }
            await ExecuteQueue(CoreEntityExecuteQueueIndex.Preload,true,new PreloadQueueInfoArg(preloadCom,opportunity));
        }
        #endregion
        
        #region Event
        public void BindEvent(GlobalEventType eventType, EventListenerDelegate callback)
        {
            if (callback == null)
            {
                FrameworkLog.LogError($"BindEvent error:callback == null!{eventType}");
                return;
            }
            _globalEventDispatcher.AddEventListener((uint)eventType,callback);
        }

        public void UnBindEvent(GlobalEventType eventType, EventListenerDelegate callback)
        {
            if (callback == null)
            {
                FrameworkLog.LogError($"UnBindEvent error:callback == null!{eventType}");
                return;
            }
            _globalEventDispatcher.RemoveEventListener((uint)eventType,callback);
        }

        public void DispatchEvent(GlobalEventType eventType,IEventArg arg)
        {
            _globalEventDispatcher.DispatchEvent((uint)eventType,arg);
        }
        #endregion
    }
}