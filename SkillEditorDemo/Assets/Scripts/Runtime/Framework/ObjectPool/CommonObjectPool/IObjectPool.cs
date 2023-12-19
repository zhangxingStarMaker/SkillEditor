namespace Module.ObjectPool
{
    public interface IObjectPool
    {
        int CountAll { get; }
        int CountActive { get; }
        int CountInactive { get; }
    }
}