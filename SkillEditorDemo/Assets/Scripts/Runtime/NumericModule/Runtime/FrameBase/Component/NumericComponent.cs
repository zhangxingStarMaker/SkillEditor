using System.Collections.Generic;
using Module.FrameBase;

namespace Runtime.NumericModule.Runtime
{
    public class NumericComponent : CoreEntity,ICoreEntityAwake
    {
        public readonly Dictionary<int, int> NumericDic = new Dictionary<int, int>();

        public int this[BattleNumericType numericType]
        {
            get
            {
                return this.GetByKey((int) numericType);
            }
            set
            {
                int v = this.GetByKey((int) numericType);
                if (v == value)
                {
                    return;
                }

                NumericDic[(int)numericType] = value;

                Update(numericType);
            }
        }

        public void OnAwake()
        {
            
        }

        public float GetAsFloat(BattleNumericType battleNumericType)
        {
            return (float)GetByKey((int)battleNumericType) / NumericConstDefine.ProportionAsIntToFloat;
        }

        public void SetValue(BattleNumericType battleNumericType, float value)
        {
            this[battleNumericType] = (int) (value * NumericConstDefine.ProportionAsIntToFloat);
        }

        public int GetAsInt(BattleNumericType battleNumericType)
        {
            return GetByKey((int)battleNumericType);
        }

        public void SetValue(BattleNumericType battleNumericType, int value)
        {
            this[battleNumericType] = value;
        }

        public void Update(BattleNumericType battleNumericType)
        {
            if (battleNumericType > BattleNumericType.MaxValue)
            {
                return;
            }

            int final = (int)battleNumericType / NumericConstDefine.TakeDefaultCoefficient;
            int baseValue = final * NumericConstDefine.TakeDefaultCoefficient + 1;
            int addValue = final * NumericConstDefine.TakeDefaultCoefficient + 2;
            int pctValue = final * NumericConstDefine.TakeDefaultCoefficient + 3;
            int finalAdd = final * NumericConstDefine.TakeDefaultCoefficient + 4;
            int finalPct = final * NumericConstDefine.TakeDefaultCoefficient + 5;
            // 一个数值可能会多种情况影响，比如速度,加个buff可能增加速度绝对值100，也有些buff增加10%速度，所以一个值可以由5个值进行控制其最终结果
            // final = (((base + add) * (100 + pct) / 100) + finalAdd) * (100 + finalPct) / 100;
            this.NumericDic[final] =
                ((this.GetByKey(baseValue) + this.GetByKey(addValue)) * (100 + this.GetByKey(pctValue)) / 100 +
                this.GetByKey(finalAdd)) * (100 + this.GetByKey(finalPct)) / 100;
        }
        
        private int GetByKey(int key)
        {
            this.NumericDic.TryGetValue(key, out var value);
            return value;
        }
    }
}