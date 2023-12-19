namespace Module.FrameBase
{
    //实体创建时调用
    public interface ICoreEntityAwake
    {
        void OnAwake();
    }
    public interface ICoreEntityAwake<T1>
    {
        void OnAwake(T1 t);
    }
    
    public interface ICoreEntityAwake<T1,T2>
    {
        void OnAwake(T1 t1,T2 t2);
    }
    
    public interface ICoreEntityAwake<T1,T2,T3>
    {
        void OnAwake(T1 t1,T2 t2,T3 t3);
    }
    
    public interface ICoreEntityAwake<T1,T2,T3,T4>
    {
        void OnAwake(T1 t1,T2 t2,T3 t3,T4 t4);
    }
}