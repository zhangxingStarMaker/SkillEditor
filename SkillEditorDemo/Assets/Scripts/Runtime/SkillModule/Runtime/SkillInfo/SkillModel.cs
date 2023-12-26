namespace Runtime.SkillModule.Runtime
{
    public class SkillModel
    {
        /// <summary>
        /// 释放条件，需要根据具体需求来定,
        /// 例如阿卡丽的技能释放需要判断当前黄条的值是否满足
        /// </summary>
        public string Condition;

        /// <summary>
        /// 技能消耗量
        /// </summary>
        public float Cost;

        /// <summary>
        /// 技能效果，可以理解为播放一个Timeline资源
        /// </summary>
        public string Effect;

        /// <summary>
        /// 当技能生效的时候给自身添加的Buff,永久性的Buff,可以理解为被动技能
        /// 例如盖伦的被动技能，当盖伦没有受到伤害一段时间后，他会迅速回复生命值。
        /// </summary>
        public int[] BuffID;
    }
}