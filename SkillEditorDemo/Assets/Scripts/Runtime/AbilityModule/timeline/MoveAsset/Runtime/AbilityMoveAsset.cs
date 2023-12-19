using UnityEngine;
using UnityEngine.Playables;

namespace AbilitySystem
{
    [System.Serializable]
    public class AbilityMoveAsset : PlayableAsset
    {
        public AbilityMoveBehaviour AbilityMoveBehaviour = new AbilityMoveBehaviour();
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<AbilityMoveBehaviour>.Create(graph,AbilityMoveBehaviour);
        }
    }
}