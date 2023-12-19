using System;
using System.Collections.Generic;
using Moudule.ETTaskCore;

namespace Module.FrameBase
{
    public interface IExecuteQueueInfoArg
    {
        
    }
    
    public abstract class ExecuteQueueInfo
    {
        public EntityExecuteOrder ExecuteOrder = EntityExecuteOrder.Normal;
        private readonly Queue<long> _entityInstanceIdList = new Queue<long>();
        
        public void AddEntity(CoreEntity newCoreEntity)
        {
            if (newCoreEntity != null)
            {
                _entityInstanceIdList.Enqueue(newCoreEntity.InstanceId);
            }
        }

        protected virtual bool OnExecute(CoreEntity tempCoreEntity,IExecuteQueueInfoArg arg)
        {
            throw new NotImplementedException();
        }

        protected async virtual ETTask<bool> OnExecuteASync(CoreEntity tempCoreEntity,IExecuteQueueInfoArg arg)
        {
            throw new NotImplementedException();
        }
        
        public void Execute(IExecuteQueueInfoArg arg)
        {
            Queue<long> queue = _entityInstanceIdList;
            int count = queue.Count;
            while (count-- > 0)
            {
                long instanceId = queue.Dequeue();
                CoreEntity component = LogicEntityCollector.Instance.Get(instanceId);
                if (component == null)
                {
                    continue;
                }
                if (component.IsDisposed)
                {
                    continue;
                }
                if (!OnExecute(component,arg))
                {
                    continue;
                }
                queue.Enqueue(instanceId);
            }
        }
        
        public async ETTask ExecuteASync(IExecuteQueueInfoArg arg)
        {
            Queue<long> queue = _entityInstanceIdList;
            int count = queue.Count;
            while (count-- > 0)
            {
                long instanceId = queue.Dequeue();
                CoreEntity component = LogicEntityCollector.Instance.Get(instanceId);
                if (component == null)
                {
                    continue;
                }
                if (component.IsDisposed)
                {
                    continue;
                }
                bool result = await OnExecuteASync(component,arg);
                if (!result)
                {
                    continue;
                }
                queue.Enqueue(instanceId);
            }
        }
    }
}