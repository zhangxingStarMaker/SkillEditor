using System;
using UnityEditor.Timeline;
using UnityEditor.Timeline.Actions;
using UnityEngine.Timeline;

namespace CameraModule.Editor
{
    [Serializable]
    public class SteCameraClipEditor : ClipEditor
    {
        public override void OnCreate(TimelineClip clip, TrackAsset track, TimelineClip clonedFrom)
        {
            CameraEditorDebug.LogError("shengchengxiangji");
            base.OnCreate(clip, track, clonedFrom);
        }
    }
}