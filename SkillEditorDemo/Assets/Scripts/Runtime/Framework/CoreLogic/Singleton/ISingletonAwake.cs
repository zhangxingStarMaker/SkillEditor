namespace Module.FrameBase
{
    public interface ISingletonAwake
    {
        void Awake();
    }
    
    public interface ISingletonAwake<T>
    {
        void Awake(T t1);
    }
}