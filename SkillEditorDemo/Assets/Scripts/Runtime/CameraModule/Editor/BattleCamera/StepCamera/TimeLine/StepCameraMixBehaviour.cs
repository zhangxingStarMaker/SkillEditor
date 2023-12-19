using System;
using UnityEngine.Playables;

namespace CameraModule.Editor
{
    public class StepCameraMixBehaviour : PlayableBehaviour
    {
        private double _preTime = 0;
        public override void OnPlayableCreate(Playable playable)
        {
            base.OnPlayableCreate(playable);
            // var cameraRoot = GameObject.Find("CameraRootNode");
            // PreviewCameraEditorManager manager;
            // if (cameraRoot == null)
            // {
            //     cameraRoot = new GameObject("PreviewCameraEditorManager");
            //     manager = cameraRoot.AddComponent<PreviewCameraEditorManager>();
            // }
            // else
            // {
            //     manager = cameraRoot.GetComponent<PreviewCameraEditorManager>();
            //     if (manager == null)
            //     {
            //         manager = cameraRoot.AddComponent<PreviewCameraEditorManager>();
            //     }
            // }
            // manager.RefreshStepCameraPreview(false);
            CameraEditorFacade.Instance.OnClear();
            CameraEditorFacade.Instance.CameraEditorEntity.PreviewCameraEditorComponent.InitPreviewCameraData();
            
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);
            
            // var cameraRoot = GameObject.Find("PreviewCameraEditorManager");
            // CameraControl cameraControl = null;
            // if (cameraRoot != null)
            // {
            //     cameraControl = cameraRoot.GetComponent<PreviewCameraEditorManager>()?.Control;
            // }
            //
            // if (cameraControl == null)
            // {
            //     return;
            // }
            double playheadTime = playable.GetGraph().GetRootPlayable(0).GetTime();
            // if (Math.Abs(_preTime - playheadTime) >= 0.5f)
            // {
            //     cameraControl.ClearPreActiveCamera();
            // }
            _preTime = playheadTime;
            // cameraControl.Update(playheadTime,Math.Max(0.02f,info.deltaTime));
            CameraEditorFacade.Instance.CameraEditorEntity.PreviewCameraEditorComponent.DriverLinkCameraProcess((float)playheadTime,Math.Max(0.02f,info.deltaTime));
        }
    }
}