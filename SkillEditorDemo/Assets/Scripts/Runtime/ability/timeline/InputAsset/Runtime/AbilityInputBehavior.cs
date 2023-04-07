using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;

namespace AbilitySystem
{
    [System.Serializable]
    public class AbilityInputBehavior : PlayableBehaviour
    {
        public enum TrackEventType
        {
            [LabelText("未选择")] None,
            [LabelText("输入监听")] InputListen,
            [LabelText("索敌碰撞")] Collision,
            [LabelText("切换到连线的下一个技能节点")] NextNode,
            [LabelText("释放Timeline中配置的其他技能")] Skill,
        }

        public bool IsFrameEvent { get; private set; }

        [OnValueChanged("OnEventTypeChange")] [Title("事件类型", "不同的选项对应不同的参数列表")] [LabelText("事件类型")]
        public TrackEventType eventType;


        // [ShowIf("@this.eventType == TrackEventType.InputListen || this.eventType == TrackEventType.NextNode", false)]
        // [Header("事件分支, 会自动根据输入判断选择哪一个事件")]
        // [LabelText("分支列表")]
        // public List<CharacterEventEnum> branchList;

        // [ShowIf("@this.eventType == TrackEventType.Skill", false)]
        // [Header("触发的关联技能")]
        // [LabelText("技能配置")]
        // public SkillData skillData;
        //
        // [ShowIf("@this.eventType == TrackEventType.Collision", false)]
        // [Header("触发BUFF")]
        // [LabelText("BUFF目标")]
        // public BuffTargetType buffTarget;


        private static Dictionary<TrackEventType, bool> _enumValueMap = new Dictionary<TrackEventType, bool>()
        {
            {TrackEventType.None, true},
            {TrackEventType.InputListen, false},
            {TrackEventType.Collision, false},
            {TrackEventType.NextNode, true},
            {TrackEventType.Skill, true},
        };

        private void OnEventTypeChange()
        {
            IsFrameEvent = _enumValueMap[eventType];
        }
    }
}