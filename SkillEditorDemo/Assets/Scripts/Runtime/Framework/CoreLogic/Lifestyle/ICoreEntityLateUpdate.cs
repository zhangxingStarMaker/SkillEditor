namespace Module.FrameBase
{
    public interface ICoreEntityLateUpdate
    {
        void OnLateUpdate();
    }
    
    public class ExecuteLateUpdateQueueInfo:ExecuteQueueInfo
    {
        protected override bool OnExecute(CoreEntity tempCoreEntity, IExecuteQueueInfoArg executeQueueInfoArg)
        {
            if (tempCoreEntity is ICoreEntityLateUpdate obj)
            {
                obj.OnLateUpdate();
                return true;
            }
            return false;
        }
    }
}