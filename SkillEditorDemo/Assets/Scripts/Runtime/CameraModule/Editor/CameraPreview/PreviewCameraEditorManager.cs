#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
namespace Module.Battle.Camera
{
    public class PreviewCameraEditorManager:MonoBehaviour
    {
        // public CameraControl Control
        // {
        //     get
        //     {
        //         if (_control == null)
        //         {
        //             _control = new CameraControl();
        //         }
        //         return _control;
        //     }
        // }
        // private CameraControl _control;
        private void RemoveAllRoot()
        {
            Transform[] transforms = FindObjectsOfType<Transform>();

            // Loop through the array
            foreach (Transform t in transforms)
            {
                // Check if the name of the gameObject is CameraRoot
                if (t.gameObject.name == "CameraRoot")
                {
                    // Destroy the gameObject
                    DestroyImmediate(t.gameObject);
                }
            }
        }

        public void RefreshStepCameraPreview(bool needRebuildGraph = true)
        {
            RemoveAllRoot();
            // if (Control != null)
            // {
            //     Control.Stop();
            // }
            // var p =  GameObject.FindObjectOfType<PlayableDirector>();
            // if (p == null)
            // {
            //     Debug.LogError("No PlayableDirector");
            //     return;
            // }
            //
            CameraAssetInfo cameraAssetInfo = ScriptableObject.CreateInstance<CameraAssetInfo>();
            // TimelineAsset timeline = p.playableAsset as TimelineAsset;
            // foreach (TrackAsset track in timeline.GetOutputTracks())
            // {
            //     foreach (var timelineClip in track.GetClips())
            //     {
            //         if (timelineClip.asset is StepCameraClip clip)
            //         {
            //             if (clip.cameraAsset != null)
            //             {
            //                 if (clip.cameraAsset.VirtualCamera != null)
            //                 {
            //                     cameraAssetInfo.cameraInfoList.Add(new OneCameraAsset(timelineClip.start,timelineClip.duration,clip.cameraAsset));
            //                 }
            //             }
            //         }
            //     }
            // }
            // Control.Start(cameraAssetInfo);
            // if(needRebuildGraph)
            //     p.RebuildGraph();
            // Control.ClearPreActiveCamera();
            // Control.Update(p.time,0);
        }

        public void RefreshCameraPreview(bool needRebuildGraph = true)
        {
            RemoveAllRoot();
            // if (Control != null)
            // {
            //     Control.Stop();
            // }
            var p =  GameObject.FindObjectOfType<PlayableDirector>();
            if (p == null)
            {
                Debug.LogError("No PlayableDirector");
                return;
            }
            CameraAssetInfo cameraAssetInfo = ScriptableObject.CreateInstance<CameraAssetInfo>();
            TimelineAsset timeline = p.playableAsset as TimelineAsset;
            foreach (TrackAsset track in timeline.GetOutputTracks())
            {
                foreach (var timelineClip in track.GetClips())
                {
                    if (timelineClip.asset is CameraPreviewClip clip)
                    {
                        if (clip.cameraAsset != null)
                        {
                            if (clip.cameraAsset.VirtualCamera != null)
                            {
                                cameraAssetInfo.cameraInfoList.Add(new OneCameraAsset(timelineClip.start,timelineClip.duration,clip.cameraAsset));
                            }
                        }
                    }
                }
            }
            // Control.Start(cameraAssetInfo);
            // if(needRebuildGraph)
            //     p.RebuildGraph();
            // Control.ClearPreActiveCamera();
            // Control.Update(p.time,0);
        }
    }
}
#endif