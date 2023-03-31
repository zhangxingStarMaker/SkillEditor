using UnityEngine;
using UnityEngine.Playables;

namespace AbilitySystem
{
    [System.Serializable]
    public class AbilityTriggerAsset : PlayableAsset
    {
        public AbilityTriggerBehaviour TriggerBehaviour = new AbilityTriggerBehaviour();

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<AbilityTriggerBehaviour>.Create(graph, TriggerBehaviour);
        }
    }
}