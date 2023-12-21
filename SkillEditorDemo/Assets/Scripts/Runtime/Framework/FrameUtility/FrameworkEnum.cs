namespace Runtime.Framework.FrameUtility
{
    public enum SceneType
    {
        None = -1,
        Process = 0,
        Client = 1,
        Current = 2,
    }
    
    public enum CoreEntityExecuteQueueIndex
    {
        None = -1,
        OnGUI,
        Update,
        LateUpdate,
        Preload,
        Max,
    }
    
    public enum GlobalEventType
    {
        None,
        
        //场景相关 1~100
        SceneStateChanged = 1, //场景状态发生变更 ,param:ChangeSceneStateEventArg
        
    }
}