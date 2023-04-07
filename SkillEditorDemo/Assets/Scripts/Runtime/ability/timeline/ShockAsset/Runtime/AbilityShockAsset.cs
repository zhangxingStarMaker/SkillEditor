using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

namespace AbilitySystem
{
    [System.Serializable]
    public class AbilityShockAsset : PlayableAsset
    {
        [FormerlySerializedAs("TriggerBehaviour")] public AbilityShockBehaviour shockBehaviour = new AbilityShockBehaviour();

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<AbilityShockBehaviour>.Create(graph, shockBehaviour);
        }
    }
}