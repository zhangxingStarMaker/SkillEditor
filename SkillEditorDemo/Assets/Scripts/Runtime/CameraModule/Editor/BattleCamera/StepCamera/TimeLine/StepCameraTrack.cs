using System;
using System.Collections.Generic;
using CameraModule.Runtime;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace CameraModule.Editor
{
    [Serializable]
    [TrackClipType(typeof(StepCameraClip))]
    public class StepCameraTrack : TrackAsset
    {
        public string CameraGroupPath = "";
        public List<CameraGroupAsset> CameraGroupAssetList = new List<CameraGroupAsset>();
        public CameraConfigureAsset CameraConfigureAsset;
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var mixer = ScriptPlayable<StepCameraMixBehaviour>.Create(graph, inputCount);
            
            // 设置混合器的数据
            return mixer;
        }
    }
}