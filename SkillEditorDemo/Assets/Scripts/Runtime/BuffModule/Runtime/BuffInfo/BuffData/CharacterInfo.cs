using UnityEngine;

namespace BuffModule.Runtime
{
    public enum MoveType
    {
        ground = 0,
        fly = 1
    }

    ///<summary>
    ///角色的数值属性部分，比如最大hp、攻击力等等都在这里
    ///这个建一个结构是因为并非只有角色有这些属性，包括装备、buff、aoe、damageInfo等都会用上
    ///</summary>
    public struct ChaProperty
    {
        ///<summary>
        ///最大生命，基本都得有，哪怕角色只有1，装备可以是0
        ///</summary>
        public int hp;

        ///<summary>
        ///攻击力
        ///</summary>
        public int attack;

        ///<summary>
        ///移动速度，他不是米/秒作为单位的，而是一个可以培养的数值。
        ///具体转化为米/秒，是需要一个规则的，所以是策划脚本 int SpeedToMoveSpeed(int speed)来返回
        ///</summary>
        public int moveSpeed;

        ///<summary>
        ///行动速度，和移动速度不同，他是增加角色行动速度，也就是变化timeline和动画播放的scale的，比如wow里面开嗜血就是加行动速度
        ///具体多少也不是一个0.2f（我这个游戏中规则设定的最快为正常速度的20%，你的游戏你自己定）到5.0f（我这个游戏设定了最慢是正常速度20%），和移动速度一样需要脚本接口返回策划公式
        ///</summary>
        public int actionSpeed;

        ///<summary>
        ///弹仓，其实相当于mp了，只是我是射击游戏所以题材需要换皮。
        ///玩家层面理解，跟普通mp上限的区别是角色这个值上限一般都是0，它来自于装备。
        ///</summary>
        public int ammo;

        ///<summary>
        ///体型圆形半径，用于移动碰撞的，单位：米
        ///这个属性因人而异，但是其实在玩法中几乎不可能经营它，只有buff可能会改变一下，所以直接用游戏中用的数据就行了，不需要转化了
        ///</summary>
        public float bodyRadius;

        ///<summary>
        ///挨打圆形半径，同体型圆形，只是用途不同，用在判断子弹是否命中的时候
        ///</summary>
        public float hitRadius;

        ///<summary>
        ///角色移动类型
        ///</summary>
        public MoveType moveType;

        public ChaProperty(
            int moveSpeed, int hp = 0, int ammo = 0, int attack = 0, int actionSpeed = 100,
            float bodyRadius = 0.25f, float hitRadius = 0.25f, MoveType moveType = MoveType.ground
        )
        {
            this.moveSpeed = moveSpeed;
            this.hp = hp;
            this.ammo = ammo;
            this.attack = attack;
            this.actionSpeed = actionSpeed;
            this.bodyRadius = bodyRadius;
            this.hitRadius = hitRadius;
            this.moveType = moveType;
        }


        public static ChaProperty zero = new ChaProperty(0, 0, 0, 0, 0, 0, 0, 0);

        ///<summary>
        ///将所有值清0
        ///<param name="moveType">移动类型设置为</param>
        ///</summary>
        public void Zero(MoveType moveType = MoveType.ground)
        {
            this.hp = 0;
            this.moveSpeed = 0;
            this.ammo = 0;
            this.attack = 0;
            this.actionSpeed = 0;
            this.bodyRadius = 0;
            this.hitRadius = 0;
            this.moveType = moveType;
        }

        //定义加法和乘法的用法，其实这个应该走脚本函数返回，抛给脚本函数多个ChaProperty，由脚本函数运作他们的运算关系，并返回结果
        public static ChaProperty operator +(ChaProperty a, ChaProperty b)
        {
            return new ChaProperty(
                a.moveSpeed + b.moveSpeed,
                a.hp + b.hp,
                a.ammo + b.ammo,
                a.attack + b.attack,
                a.actionSpeed + b.actionSpeed,
                a.bodyRadius + b.bodyRadius,
                a.hitRadius + b.hitRadius,
                a.moveType == MoveType.fly || b.moveType == MoveType.fly ? MoveType.fly : MoveType.ground
            );
        }

        public static ChaProperty operator *(ChaProperty a, ChaProperty b)
        {
            return new ChaProperty(
                Mathf.RoundToInt(a.moveSpeed * (1.0000f + Mathf.Max(b.moveSpeed, -0.9999f))),
                Mathf.RoundToInt(a.hp * (1.0000f + Mathf.Max(b.hp, -0.9999f))),
                Mathf.RoundToInt(a.ammo * (1.0000f + Mathf.Max(b.ammo, -0.9999f))),
                Mathf.RoundToInt(a.attack * (1.0000f + Mathf.Max(b.attack, -0.9999f))),
                Mathf.RoundToInt(a.actionSpeed * (1.0000f + Mathf.Max(b.actionSpeed, -0.9999f))),
                a.bodyRadius * (1.0000f + Mathf.Max(b.bodyRadius, -0.9999f)),
                a.hitRadius * (1.0000f + Mathf.Max(b.hitRadius, -0.9999f)),
                a.moveType == MoveType.fly || b.moveType == MoveType.fly ? MoveType.fly : MoveType.ground
            );
        }

        public static ChaProperty operator *(ChaProperty a, float b)
        {
            return new ChaProperty(
                Mathf.RoundToInt(a.moveSpeed * b),
                Mathf.RoundToInt(a.hp * b),
                Mathf.RoundToInt(a.ammo * b),
                Mathf.RoundToInt(a.attack * b),
                Mathf.RoundToInt(a.actionSpeed * b),
                a.bodyRadius * b,
                a.hitRadius * b,
                a.moveType
            );
        }
    }
    
    ///<summary>
    ///角色的可操作状态，这个是根据游戏玩法来细节设计的，目前就用这个demo需要的
    ///</summary>
    public struct ChaControlState{
        ///<summary>
        ///是否可以移动坐标
        ///</summary>
        public bool canMove;

        ///<summary>
        ///是否可以转身
        ///</summary>
        public bool canRotate;

        ///<summary>
        ///是否可以使用技能，这里的是“使用技能”特指整个技能流程是否可以开启
        ///如果是类似中了沉默，则应该走buff的onCast，尤其是类似wow里面沉默了不能施法但是还能放致死打击（部分技能被分类为法术，会被沉默，而不是法术的不会）
        ///</summary>
        public bool canUseSkill;

        public ChaControlState(bool canMove = true, bool canRotate = true, bool canUseSkill = true){
            this.canMove = canMove;
            this.canRotate = canRotate;
            this.canUseSkill = canUseSkill;
        }

        public void Origin(){
            this.canMove = true;
            this.canRotate = true;
            this.canUseSkill = true;
        }

        public static ChaControlState origin = new ChaControlState(true, true, true);

        ///<summary>
        ///昏迷效果
        ///</summary>
        public static ChaControlState stun = new ChaControlState(false, false, false);

        public static ChaControlState operator +(ChaControlState cs1, ChaControlState cs2){
            return new ChaControlState(
                cs1.canMove & cs2.canMove,
                cs1.canRotate & cs2.canRotate,
                cs1.canUseSkill & cs2.canUseSkill
            );
        }
    }
}