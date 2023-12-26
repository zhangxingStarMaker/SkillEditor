using System.Collections.Generic;
using UnityEngine;

namespace BuffModule.Runtime
{
    /// <summary>
    /// 这是创建BuffObj的中间件，确切地说，我们每次要给角色的Buff做增删改查，都应该创建一条AddBuffInfo，而非直接修改ChaState中的buff。
    /// 因为我们可能有些技能的效果会导致角色的添加buff，比如说有一个buff的效果是“受到伤害的时候获得厚皮（另一个buff）”，“厚皮”的效果是受到伤害时降低50%，
    /// 并且下一次伤害免疫（又是一个新的buff），这时候我们如果同一轮里面执行就会执行到后面2个新增加的buff效果，但实际上这是不应该发生的，
    /// 只是恰好C#的list管理方式碰巧有这个效果在那里，但这非常的不安全。所以我们正确的做法，就是当要给角色添加buff的时候，
    /// 一定是产生一个AddBuffInfo，由BuffManager来管理buff的添加
    /// </summary>
    public class AddBuffInfoEntity
    {
        ///<summary>
        ///buff的负责人是谁，可以是null
        ///</summary>
        public GameObject caster;

        ///<summary>
        ///buff要添加给谁，这个必须有
        ///</summary>
        public GameObject target;

        ///<summary>
        ///buff的model，这里当然可以从数据里拿，也可以是逻辑脚本现生成的
        ///</summary>
        public BuffData buffModel;

        ///<summary>
        ///要添加的层数，负数则为减少
        ///</summary>
        public int addStack;

        ///<summary>
        ///关于时间，是改变还是设置为, true代表设置为，false代表改变
        ///</summary>
        public bool durationSetTo;

        ///<summary>
        ///是否是一个永久的buff，即便=true，时间设置也是有意义的，因为时间如果被减少到0以下，即使是永久的也会被删除
        ///</summary>
        public bool permanent;

        ///<summary>
        ///时间值，设置为这个值，或者加上这个值，单位：秒
        ///</summary>
        public float duration;

        ///<summary>
        ///buff的一些参数，这些参数是逻辑使用的，比如wow中牧师的盾还能吸收多少伤害，就可以记录在buffParam里面
        ///</summary>
        public Dictionary<string, object> buffParam;

        public AddBuffInfoEntity(
            BuffData model, GameObject caster, GameObject target,
            int stack, float duration, bool durationSetTo = true,
            bool permanent = false,
            Dictionary<string, object> buffParam = null
        ){
            this.buffModel = model;
            this.caster = caster;
            this.target = target;
            this.addStack = stack;
            this.duration = duration;
            this.durationSetTo = durationSetTo;
            this.buffParam = buffParam;
            this.permanent = permanent;
        }
    }
}