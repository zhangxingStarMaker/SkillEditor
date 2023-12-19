using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

namespace AbilitySystem
{
    [LabelText("添加顿帧内容")]
    [System.Serializable]
    public class AbilityPauseFrameAsset : PlayableAsset
    {
        [FormerlySerializedAs("TriggerBehaviour")] public AbilityPauseFrameBehaviour pauseFrameBehaviour = new AbilityPauseFrameBehaviour();

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<AbilityPauseFrameBehaviour>.Create(graph, pauseFrameBehaviour);
        }
    }
}