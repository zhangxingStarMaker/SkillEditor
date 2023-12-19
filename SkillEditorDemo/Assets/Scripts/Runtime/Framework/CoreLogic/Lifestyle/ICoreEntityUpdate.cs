namespace Module.FrameBase
{
    public interface ICoreEntityUpdate
    {
        void OnUpdate();
    }

    public class ExecuteUpdateQueueInfo:ExecuteQueueInfo
    {
        protected override bool OnExecute(CoreEntity tempCoreEntity, IExecuteQueueInfoArg executeQueueInfoArg)
        {
            if (tempCoreEntity is ICoreEntityUpdate updateObj)
            {
                updateObj.OnUpdate();
                return true;
            }
            return false;
        }
    }
}