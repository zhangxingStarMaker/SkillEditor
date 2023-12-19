using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace AbilitySystem
{
    [LabelText("添加顿帧轨道")]
    [TrackClipType(typeof(AbilityPauseFrameAsset))]
    public class AbilityPauseFrameTrack : TrackAsset
    {
        [HideInInspector]
        public TrackAsset CurrentTrackRelyOnTrackAsset;
        protected override Playable CreatePlayable(PlayableGraph graph, GameObject gameObject, TimelineClip clip)
        {
            return base.CreatePlayable(graph, gameObject, clip);
        }
    }
}