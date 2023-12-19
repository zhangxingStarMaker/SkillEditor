using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace AbilitySystem
{
    [TrackClipType(typeof(AbilityShockAsset))]
    public class AbilityShockTrack : TrackAsset
    {
        [HideInInspector]
        public TrackAsset CurrentTrackRelyOnTrackAsset;
        protected override Playable CreatePlayable(PlayableGraph graph, GameObject gameObject, TimelineClip clip)
        {
            return base.CreatePlayable(graph, gameObject, clip);
        }
    }
}