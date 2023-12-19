#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
namespace CameraModule.Runtime
{
    [TrackColor(0.8113f, 0.8301f, 0.6461f)]
    [TrackClipType(typeof(CameraPreviewClip))]
    public class CameraPreviewTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var mixer = ScriptPlayable<CameraPreviewMixerBehaviour>.Create(graph, inputCount);
            
            // 设置混合器的数据
            return mixer;
        }
    }
}
#endif