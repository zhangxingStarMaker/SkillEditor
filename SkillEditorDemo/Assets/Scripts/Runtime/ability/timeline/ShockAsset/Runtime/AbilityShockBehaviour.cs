using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

namespace AbilitySystem
{
    [System.Serializable]
    public class AbilityShockBehaviour : PlayableBehaviour
    {
        public enum TriggerEventType
        {
            [LabelText("顿帧")] StutterFrame,
            [LabelText("震屏")] ShakeScreen,
        }

        public enum TriggerCondition
        {
            [LabelText("自动触发")] None,
            [LabelText("碰撞到目标")] Npc
        }


        public TriggerCondition Condition;

        [LabelText("触发数量")]
        public int NpcCount;

        [LabelText("命中触发")]
        public bool HitTrigger = false;

        [LabelText("震动方向")]
        public Vector3 ShakeDirection = new Vector3(0, -1, 0);

        [LabelText("震动强度")]
        public float ShakeGain = 1f;

        [LabelText("持续时间")]
        public float ShakeTime = 0.2f;
    }
}