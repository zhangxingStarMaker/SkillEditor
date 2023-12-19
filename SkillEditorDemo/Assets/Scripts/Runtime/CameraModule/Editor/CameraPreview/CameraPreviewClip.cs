#if UNITY_EDITOR
using CameraModule.Runtime;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Module.Battle.Camera
{
    public class CameraPreviewClip : PlayableAsset, ITimelineClipAsset
    {
        public CameraAsset cameraAsset;
        public CameraData CameraData;
        public ClipCaps clipCaps => ClipCaps.Blending;
        
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<CameraPreviewBehaviour>.Create(graph);
            return playable;
        }
    }
}
#endif