using UnityEngine;
using UnityEngine.Playables;

namespace AbilitySystem
{
    [System.Serializable]
    public class AbilityCollisionAsset : PlayableAsset
    {
        public AbilityCollisionBehaviour CollisionBehaviour = new AbilityCollisionBehaviour();

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<AbilityCollisionBehaviour>.Create(graph, CollisionBehaviour);
        }
    }
}