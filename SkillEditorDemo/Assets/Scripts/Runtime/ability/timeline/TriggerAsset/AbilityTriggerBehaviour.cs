using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

namespace AbilitySystem
{
    [System.Serializable]
    public class AbilityTriggerBehaviour : PlayableBehaviour
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

        public TriggerEventType EventType;

        [ShowIf("@this.EventType == TriggerEventType.StutterFrame")]
        public TriggerCondition Condition;

        [ShowIf("@this.EventType == TriggerEventType.StutterFrame && this.Condition == TriggerCondition.Npc")]
        [LabelText("触发数量")]
        public int NpcCount;

        [ShowIf("@this.EventType == TriggerEventType.ShakeScreen")] [LabelText("命中触发")]
        public bool HitTrigger = false;

        [ShowIf("@this.EventType == TriggerEventType.ShakeScreen")] [LabelText("震动方向")]
        public Vector3 ShakeDirection = new Vector3(0, -1, 0);

        [ShowIf("@this.EventType == TriggerEventType.ShakeScreen")] [LabelText("震动强度")]
        public float ShakeGain = 1f;

        [ShowIf("@this.EventType == TriggerEventType.ShakeScreen")] [LabelText("持续时间")]
        public float ShakeTime = 0.2f;
    }
}