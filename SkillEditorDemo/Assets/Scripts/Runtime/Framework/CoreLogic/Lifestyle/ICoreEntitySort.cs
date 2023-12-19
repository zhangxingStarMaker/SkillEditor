namespace Module.FrameBase
{
    public enum EntityExecuteOrder
    {
        Normal = 5000,
    }
    
    public interface ICoreEntitySort
    {
        EntityExecuteOrder GetSortId();
    }
}