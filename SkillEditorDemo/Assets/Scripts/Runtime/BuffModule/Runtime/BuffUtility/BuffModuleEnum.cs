namespace BuffModule.Runtime
{
    public enum BuffPriority
    {
        None = 0,
        Normal = 1,
        MidLevel = 2,
        HighLevel = 3,
        Special = 4,
    }

    /// <summary>
    /// 当增加buff进行叠加的时候处理
    /// </summary>
    public enum BuffAddTimeUpdate
    {
        None = 0,
        Add = 1,
        Replace = 2,
        Keep = 3,
    }

    /// <summary>
    /// 当Buff移除时处理
    /// </summary>
    public enum BuffRemoveStackUpdate
    {
        None = 0,
        Keep = 1,
        Reduce = 2,
        Clear = 3,
        
    }

    /// <summary>
    /// 当前状态
    /// </summary>
    public enum BuffState
    {
        None = 0,
    }
    
}