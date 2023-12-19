#if UNITY_EDITOR
using System;
using CameraModule.Editor;
using UnityEngine.Playables;
namespace Module.Battle.Camera
{
    public class CameraPreviewMixerBehaviour : PlayableBehaviour
    {
        private double _preTime = 0;
        public override void OnPlayableCreate(Playable playable)
        {
            CameraEditorFacade.Instance.OnClear();
            CameraEditorFacade.Instance.CameraEditorEntity.PreviewCameraEditorComponent.InitAllPreviewCameraData();
        }

        // 实现PrepareFrame方法
        public override void PrepareFrame(Playable playable, FrameData info)
        {
        }
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            double playheadTime = playable.GetGraph().GetRootPlayable(0).GetTime();
            _preTime = playheadTime;
            CameraEditorFacade.Instance.CameraEditorEntity.PreviewCameraEditorComponent.DriverLinkCameraProcess((float)playheadTime,info.deltaTime);
        }
    }
}
#endif