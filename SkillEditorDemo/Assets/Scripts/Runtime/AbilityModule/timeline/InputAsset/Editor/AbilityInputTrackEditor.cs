using JetBrains.Annotations;
using UnityEditor.Timeline;
using UnityEngine.Timeline;

namespace AbilitySystem
{
    [UsedImplicitly]
    [CustomTimelineEditor(typeof(AbilityInputTrack))]
    public class AbilityInputTrackEditor : AbilityTrackEditor
    {
        public override void OnCreate(TrackAsset track, TrackAsset copiedFrom)
        {
            base.OnCreate(track, copiedFrom);
            SetTrackName(track,"输入轨道");
        }
    }
}