using JetBrains.Annotations;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

namespace AbilitySystem
{
    [UsedImplicitly]
    [CustomTimelineEditor(typeof(AbilityMoveTrack))]
    public class AbilityMoveTrackEditor : AbilityTrackEditor
    {
        public override void OnCreate(TrackAsset track, TrackAsset copiedFrom)
        {
            base.OnCreate(track, copiedFrom);
            SetTrackName(track,"移动轨道");
        }

        public override Object GetBindingFrom(Object candidate, TrackAsset track)
        {
            return base.GetBindingFrom(candidate, track);
        }
    }
}