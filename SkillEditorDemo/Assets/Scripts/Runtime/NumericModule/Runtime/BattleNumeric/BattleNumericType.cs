namespace Runtime.NumericModule.Runtime
{
    public enum BattleNumericType
    {
        MaxValue = 10000,
        
        /// <summary>
        /// 速度编号
        /// </summary>
        Speed = 1000,
        /// <summary>
        /// 速度base
        /// </summary>
        SpeedBase = Speed * NumericConstDefine.TakeDefaultCoefficient + 1,
        /// <summary>
        /// 速度增加
        /// </summary>
        SpeedAdd = Speed * NumericConstDefine.TakeDefaultCoefficient+2,
        /// <summary>
        /// 速度增加百分比
        /// </summary>
        SpeedPct = Speed *NumericConstDefine.TakeDefaultCoefficient +3,
        /// <summary>
        /// 最终速度增加值
        /// </summary>
        SpeedFinalAdd = Speed * NumericConstDefine.TakeDefaultCoefficient + 4,
        /// <summary>
        /// 最终速度增加百分比
        /// </summary>
        SpeedFinalPct = Speed * NumericConstDefine.TakeDefaultCoefficient +5,
        
        /// <summary>
        /// 暂不知
        /// </summary>
        Hp = 1001,
        /// <summary>
        /// 基础生命值
        /// </summary>
        HpBase = Hp * NumericConstDefine.TakeDefaultCoefficient +1,
        /// <summary>
        /// 最大生命值
        /// </summary>
        MaxHp = 1002,
        /// <summary>
        /// 基础生命最大值,可能根据等级来限制
        /// </summary>
        MaxHpBase = MaxHp * NumericConstDefine.TakeDefaultCoefficient + 1,
        /// <summary>
        /// 最大生命值增加
        /// </summary>
        MaxHpAdd = MaxHp * NumericConstDefine.TakeDefaultCoefficient + 2,
        /// <summary>
        /// 最大生命值增加百分比
        /// </summary>
        MaxHpPct = MaxHp * NumericConstDefine.TakeDefaultCoefficient + 3,
        /// <summary>
        /// 最大生命值最后增加
        /// </summary>
        MaxHpFinalAdd = MaxHp * NumericConstDefine.TakeDefaultCoefficient + 4,
        /// <summary>
        /// 最大生命值最后增加百分比
        /// </summary>
        MaxHpFinalPct = MaxHp * NumericConstDefine.TakeDefaultCoefficient + 5,
    }
}