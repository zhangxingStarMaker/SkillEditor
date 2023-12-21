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
    public enum BuffRunningState
    {
        None = 0,
        /// <summary>
        /// 激活状态
        /// </summary>
        Active = 1, 
        /// <summary>
        /// 冷却状态，不可用
        /// </summary>
        Cooldown = 2, 
        /// <summary>
        /// 中断或者挂起状态
        /// </summary>
        Suspended = 3,
        /// <summary>
        /// 已经处于叠加状态
        /// </summary>
        Stacking = 4,
        /// <summary>
        /// 效果过期或者时间结束,可移除状态
        /// </summary>
        Expired = 5,
    }
    
}