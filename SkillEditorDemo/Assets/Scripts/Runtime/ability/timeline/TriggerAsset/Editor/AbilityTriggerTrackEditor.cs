using JetBrains.Annotations;
using UnityEditor.Timeline;
using UnityEngine.Timeline;

namespace AbilitySystem
{
    [UsedImplicitly]
    [CustomTimelineEditor(typeof(AbilityTriggerTrack))]
    public class AbilityTriggerTrackEditor : AbilityTrackEditor
    {
        public override void OnCreate(TrackAsset track, TrackAsset copiedFrom)
        {
            base.OnCreate(track, copiedFrom);
            SetTrackName(track);
        }

        /// <summary>
        /// 设置轨道名字
        /// </summary>
        protected void SetTrackName(TrackAsset track)
        {
            int index = 1;
            var trackAssets = track.timelineAsset.GetOutputTracks();
            TrackAsset lastTrackAsset = null;
            foreach (var trackAsset in trackAssets)
            {
                if (trackAsset is AbilityTriggerTrack && trackAsset != track)
                {
                    lastTrackAsset = trackAsset;
                }
            }

            if (lastTrackAsset!=null)
            {
                string[] names = lastTrackAsset.name.Split(':');
                index = int.Parse(names[1])+1;
            }
            
            track.name = "碰撞检测轨道 : "+index;
        }
    }
}