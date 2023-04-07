using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;


namespace AbilitySystem
{
    [TrackClipType(typeof(AbilityInputAsset))]
    public class AbilityInputTrack : TrackAsset
    {
        public AbilityInputMixer inputMinxer;
    
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<AbilityInputMixer>.Create(graph, inputMinxer, inputCount);
        }
    }
}


