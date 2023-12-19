using JetBrains.Annotations;
using UnityEditor.Timeline;
using UnityEngine.Timeline;

namespace AbilitySystem
{
    [UsedImplicitly]
    [CustomTimelineEditor(typeof(AbilityPauseFrameTrack))]
    public class AbilityPauseFrameTrackEditor : AbilityTrackEditor
    {
        public override void OnCreate(TrackAsset track, TrackAsset copiedFrom)
        {
            base.OnCreate(track, copiedFrom);
            SetTrackName(track,"顿帧效果轨道");
        }
    }
}