namespace BuffModule.Runtime
{
    public class BuffItem
    {
        #region 基本信息

        public uint ID;
        
        /// <summary>
        /// 是一个string数组，他是策划定义的内容，因为这跟游戏玩法逻辑息息相关。比如我们游戏中有“中毒效果”，那么策划就要定义一个"poison"，
        /// 所有认为是中毒效果的buff的tag里面都得有这个。这样当出现一个技能，它的效果是“清除所有中毒效果”的时候，计算机才能“大概明白”什么是“中毒效果”——
        /// 这相当于是对于buff的一个属性的描述。
        /// </summary>
        public string[] Tags;
        
        public string BuffName;
        public string Description;
        public string IconPath;
        
        /// <summary>
        /// 最大层数，这通常和游戏的buff堆叠规则有关。比如在《激战2》里，所有buff的maxStack都是1，所以不需要这个属性。
        /// 《激战2》中，虽然我们肉眼看到比如“流血”的buff有很多层，但实际上他只是UI统计了角色身上有多少个model.id==“流血”的BuffObj，作
        /// 为一个数字显示出来而已，所以只是个UI的设计，而非buff本身需要maxStack。
        /// </summary>
        public uint MaxStack;//最大叠加次数
        
        /// <summary>
        /// 优先级，这通常是一个int，也是需要策划预设好的。在我们添加buff的时候需要根据这个来进行一次排序（buff列表的排序），
        /// 而执行buff逻辑的时候是[0, 列表长度)的顺序执行的，这是非常有必要的。比如我们有2个buff，A的效果是伤害无效化，
        /// 即DamageInfo的damage全部清0；B的效果是将受到的伤害反弹给攻击者，也就是创建一个新的DamageInfo，攻击者防御者换位，damage=现在的damage。
        /// 这时候问题就来了，如果有人对“我”造成50暗影伤害，我应该反弹50暗影伤害，还是0伤害？也就是A和B的执行顺序问题决定了最终结果，因此到底反弹50还是0，
        /// 这是由策划说了算的——策划填写的buff的priority和程序实现的排序规则，决定了最终谁先运行。
        /// </summary>
        public BuffPriority BuffPriority;//执行优先级
        
        ///<summary>
        ///buff会给角色添加的属性，在实际游戏开发中给角色添加属性的buff不会太多，
        /// 例如：LOL中的大龙Buff、远古龙Buff
        ///</summary>
        public ChaProperty[] propMod;

        ///<summary>
        ///buff对于角色的ChaControlState的影响,
        /// 例如：冰冻、击飞、眩晕导致角色不能移动
        ///</summary>
        public ChaControlState stateMod;
        

        #endregion
        
        #region 时间属性
        
        public bool IsPermanent;//永久性
        public float DurationTime;//持续时间
        
        /// <summary>
        /// 这和onTick的回调点是配套的，如果这个是<=0的数字，或者回调点是空 那么就不会有OnTick的事件发生了
        /// </summary>
        public float TickTime;//执行一次的时间
        
        #endregion
        
        #region 时间属性

        public BuffAddTimeUpdate BuffAddTimeUpdate; //更新方式
        public BuffRemoveStackUpdate BuffRemoveStackUpdate;//移除方式

        #endregion

    }
}