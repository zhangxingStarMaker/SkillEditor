using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace AbilitySystem
{
    [System.Serializable]
    public class AbilityInputAsset : PlayableAsset
    {
        public AbilityInputBehavior InputInfo = new AbilityInputBehavior();

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<AbilityInputBehavior>.Create(graph, InputInfo);
        }
    }
}