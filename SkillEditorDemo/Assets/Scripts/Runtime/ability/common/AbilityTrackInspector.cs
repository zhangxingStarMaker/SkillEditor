using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.Serialization;
using UnityEngine.Timeline;

namespace AbilitySystem
{
    public class AbilityTrackInspector : Editor
    {
        [FormerlySerializedAs("TrackAsset")] public TrackAsset CurrentTrackAsset;
        [FormerlySerializedAs("CurrentTrackAssets")] public List<TrackAsset> CurrentTimelineTrackAssets = new List<TrackAsset>();
        private int _selectTrackIndex = 0;
        private int _selectClipIndex = 0;
        public TrackAsset SelectTrackAsset = null;
        public TimelineClip SelectTimelineClip = null;
        

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
            CurrentTimelineTrackAssets.Add(null);
            foreach (var trackAsset in trackAssets)
            {
                if (trackAsset is not MarkerTrack && trackAsset!=CurrentTrackAsset)
                {
                    CurrentTimelineTrackAssets.Add(trackAsset);
                }
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ShowSelectTrack();
            ShowSelectClip();
        }

        /// <summary>
        /// 显示选择的轨道数据
        /// </summary>
        private void ShowSelectTrack()
        {
            if (CurrentTimelineTrackAssets!=null&& CurrentTimelineTrackAssets.Count>0)
            {
                List<string> trackAssetNames = new List<string>();
                foreach (var trackAsset in CurrentTimelineTrackAssets)
                {
                    if (trackAsset==null)
                    {
                        trackAssetNames.Add("无选择");
                    }
                    else
                    {
                        trackAssetNames.Add(trackAsset.name);
                    }
                }
                _selectTrackIndex = EditorGUILayout.Popup("选择依赖赛道",_selectTrackIndex, trackAssetNames.ToArray());
                SelectTrackAsset = CurrentTimelineTrackAssets[_selectTrackIndex];
            }
        }

        /// <summary>
        /// 挑选选中的Track中的Clip的数据
        /// </summary>
        private void ShowSelectClip()
        {
            if (SelectTrackAsset!=null)
            {
                var trackClips = SelectTrackAsset.GetClips().ToArray();
                if (trackClips.Length>0)
                {
                    List<string> trackClipNames = new List<string>(); 
                    foreach (var trackClip in trackClips)
                    {
                        trackClipNames.Add(trackClip.displayName);
                        _selectClipIndex = EditorGUILayout.Popup("选择依赖赛道", _selectClipIndex, trackClipNames.ToArray());
                    }
                    SelectTimelineClip = trackClips[_selectClipIndex];
                }
            }
        }
    }
}