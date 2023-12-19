using System;
using Module.Battle.Camera;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace CameraModule.Editor
{
    [Serializable]
    public class StepCameraClip : PlayableAsset, ITimelineClipAsset
    {
        public CameraAsset cameraAsset;
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<StepCameraBehaviour>.Create(graph);
            // OnCreateDefaultAsset();
            return playable;
        }

        private void OnCreateDefaultAsset()
        {
            cameraAsset = CameraEditorFacade.Instance.CameraEditorEntity.CreateDefaultCameraAsset();
        }
        

        public ClipCaps clipCaps => ClipCaps.Blending;
    }
}