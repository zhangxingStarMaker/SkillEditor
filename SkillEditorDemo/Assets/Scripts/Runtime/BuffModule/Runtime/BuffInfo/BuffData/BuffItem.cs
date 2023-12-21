namespace BuffModule.Runtime
{
    public class BuffItem
    {
        #region 基本信息

        public uint ID;
        public string BuffName;
        public string Description;
        public string IconPath;
        public uint MaxStack;//最大叠加次数
        public BuffPriority BuffPriority;//执行优先级

        #endregion
        
        #region 时间属性
        
        public bool IsPermanent;//永久性
        public float DurationTime;//持续时间
        public float TickTime;//执行一次的时间
        
        #endregion
        
        #region 时间属性

        public BuffAddTimeUpdate BuffAddTimeUpdate; //更新方式
        public BuffRemoveStackUpdate BuffRemoveStackUpdate;//移除方式

        #endregion

    }
}