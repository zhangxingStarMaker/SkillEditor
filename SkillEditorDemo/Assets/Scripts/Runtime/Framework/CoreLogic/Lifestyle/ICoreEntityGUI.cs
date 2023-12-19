namespace Module.FrameBase
{
    public interface ICoreEntityGUI
    {
        void OnGUI();
    }
    
    public class ExecuteGUIQueueInfo:ExecuteQueueInfo
    {
        protected override bool OnExecute(CoreEntity tempCoreEntity, IExecuteQueueInfoArg executeQueueInfoArg)
        {
            if (tempCoreEntity is ICoreEntityGUI obj)
            {
                obj.OnGUI();
                return true;
            }
            return false;
        }
    }
}