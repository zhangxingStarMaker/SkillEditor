using UnityEditor.Timeline;
using UnityEngine.Timeline;

namespace AbilitySystem
{
    public class AbilityTrackEditor : TrackEditor
    {
        public override void OnCreate(TrackAsset track, TrackAsset copiedFrom)
        {
            base.OnCreate(track, copiedFrom);
        }
        
        /// <summary>
        /// 设置轨道名字
        /// </summary>
        protected void SetTrackName(TrackAsset track,string name)
        {
            int index = 1;
            var trackAssets = track.timelineAsset.GetOutputTracks();
            TrackAsset lastTrackAsset = null;
            foreach (var trackAsset in trackAssets)
            {
                if (track.GetType() == trackAsset.GetType() && trackAsset != track)
                {
                    lastTrackAsset = trackAsset;
                }
            }

            if (lastTrackAsset!=null)
            {
                string[] names = lastTrackAsset.name.Split(':');
                index = int.Parse(names[1])+1;
            }
            
            track.name = name + " : "+index;
        }
    }
}