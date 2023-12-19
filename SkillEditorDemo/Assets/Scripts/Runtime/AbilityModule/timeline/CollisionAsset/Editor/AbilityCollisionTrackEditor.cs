using JetBrains.Annotations;
using UnityEditor.Timeline;
using UnityEngine.Timeline;

namespace AbilitySystem
{
    [UsedImplicitly]
    [CustomTimelineEditor(typeof(AbilityCollisionTrack))]
    public class AbilityCollisionTrackEditor : AbilityTrackEditor
    {
        public override void OnCreate(TrackAsset track, TrackAsset copiedFrom)
        {
            base.OnCreate(track, copiedFrom);
            SetTrackName(track,"碰撞检测轨道");
        }
    }
}