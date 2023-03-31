using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.Serialization;
using UnityEngine.Timeline;

namespace AbilitySystem
{
    public class AbilityTrackInspector : Editor
    {
        [FormerlySerializedAs("TrackAsset")] public TrackAsset CurrentTrackAsset;
        [FormerlySerializedAs("CurrentTrackAssets")] public List<TrackAsset> CurrentTimelineTrackAssets = new List<TrackAsset>();
        private int _selectIndex = 0;
        public TrackAsset SelectTrackAsset = null;
        

        private void OnEnable()
        {
            OnSelectTrack();
        }

        /// <summary>
        /// 选中当前赛道后调用的方法
        /// </summary>
        protected virtual void OnSelectTrack()
        {
            CurrentTrackAsset = (TrackAsset)this.target;
            CurrentTimelineTrackAssets.Clear();
            var trackAssets = CurrentTrackAsset.timelineAsset.GetOutputTracks();
            foreach (var trackAsset in trackAssets)
            {
                if (trackAsset is not MarkerTrack && trackAsset!=CurrentTrackAsset)
                {
                    CurrentTimelineTrackAssets.Add(trackAsset);
                }
            }

            if (CurrentTimelineTrackAssets.Count>0)
            {
                SelectTrackAsset = CurrentTimelineTrackAssets[_selectIndex];
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (CurrentTimelineTrackAssets!=null&& CurrentTimelineTrackAssets.Count>0)
            {
                List<string> trackAssetNames = new List<string>();
                foreach (var trackAsset in CurrentTimelineTrackAssets)
                {
                    trackAssetNames.Add(trackAsset.name);
                }
                _selectIndex = EditorGUILayout.Popup("选择依赖赛道",_selectIndex, trackAssetNames.ToArray());
                SelectTrackAsset = CurrentTimelineTrackAssets[_selectIndex];
            }
        }
    }
}