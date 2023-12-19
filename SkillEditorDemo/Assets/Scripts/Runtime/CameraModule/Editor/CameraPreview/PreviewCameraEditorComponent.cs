using System.Collections.Generic;
using CameraModule.Runtime;
using CameraModule.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Object = UnityEngine.Object;

namespace CameraModule.Editor
{
    public class PreviewCameraEditorComponent : EditorComponent
    {
        private bool _hasRole = true;

        public override void OnInit(CameraEditorEntity cameraEditorEntity)
        {
            base.OnInit(cameraEditorEntity);
        }

        public void CreatePreviewCameraTrack(CameraPreviewTrack cameraPreviewTrack,List<CameraData> cameraDataList)
        {
            if (cameraPreviewTrack!=null)
            {
                foreach (var timelineClip in cameraPreviewTrack.GetClips())
                {
                    cameraPreviewTrack.DeleteClip(timelineClip);
                }
            }
            foreach (var cameraData in cameraDataList)
            {
                var timelineClip = cameraPreviewTrack.CreateClip<CameraPreviewClip>();
                var cameraPreviewClip = timelineClip.asset as CameraPreviewClip;
                if (cameraPreviewClip == null)
                {
                    continue;
                }

                cameraPreviewClip.cameraAsset = cameraData.CameraAsset;
                cameraPreviewClip.CameraData = cameraData;
                timelineClip.start = cameraData.StartTime;
                timelineClip.duration = cameraData.DurationTime;
            }

            InitAllPreviewCameraData();
        }

        public void InitAllPreviewCameraData()
        {
            var p =  Object.FindObjectOfType<PlayableDirector>();
            if (p == null)
            {
                CameraEditorDebug.LogError("No PlayableDirector");
                return;
            }
            
            GameObject roleObj =  GameObject.Find("ChoreographyPlayer");
            if (roleObj==null)
            {
                CameraEditorDebug.LogError("没有找到主角");
                _hasRole = false;
                return;
            }

            List<CameraData> cameraDataList = new List<CameraData>();

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
                                CameraData cameraData = clip.CameraData;
                                cameraData.AssetName = clip.cameraAsset.name;
                                cameraData.StartTime = (float)timelineClip.start;
                                cameraData.DurationTime = (float) timelineClip.duration;
                                cameraData.CameraAsset = clip.cameraAsset;
                                cameraData.TargetTransform = roleObj.transform;
                                cameraDataList.Add(cameraData);
                            }
                        }
                    }
                }
            }
            CameraEditorDebug.LogInfo("Editor Camera Init："+cameraDataList.Count);
            // CameraEditorEntity.CameraEditorManager.CameraCameraContextCtrl.OnInitLinkCameraControl(cameraDataList,null);
            // SavePreviewCameraDataList(cameraDataList);
        }

        private void SavePreviewCameraDataList(List<CameraData> cameraDataList)
        {
            CameraGmAsset cameraGmAsset = AssetDatabase.LoadAssetAtPath<CameraGmAsset>(CameraEditorDefine.CameraGmPath);
            if (cameraGmAsset!=null)
            {
                cameraGmAsset.EditorCameraDataList.Clear();
                foreach (var cameraData in cameraDataList)
                {
                    CameraData newCameraData = new CameraData();
                    newCameraData.Index = cameraData.Index;
                    newCameraData.Name = cameraData.Name;
                    newCameraData.Pos = cameraData.Pos;
                    newCameraData.Rotation = cameraData.Rotation;
                    newCameraData.AssetName = cameraData.AssetName;
                    newCameraData.CameraAsset = cameraData.CameraAsset;
                    newCameraData.TargetTransform = cameraData.TargetTransform;
                    newCameraData.StartTime = cameraData.StartTime;
                    newCameraData.EndTime = cameraData.EndTime;
                    newCameraData.DurationTime = cameraData.DurationTime;
                    cameraGmAsset.EditorCameraDataList.Add(newCameraData);
                }
                
                EditorUtility.SetDirty(cameraGmAsset);
                AssetDatabase.SaveAssetIfDirty(cameraGmAsset);
            }
            else
            {
                CameraEditorDebug.LogError("Camera Gm Asset Is Null");
            }
        }

        public void InitPreviewCameraData()
        {
            var p =  Object.FindObjectOfType<PlayableDirector>();
            if (p == null)
            {
                CameraEditorDebug.LogError("No PlayableDirector");
                return;
            }
            
            GameObject roleObj =  GameObject.Find("ChoreographyPlayer");
            if (roleObj==null)
            {
                CameraEditorDebug.LogError("没有找到主角");
                _hasRole = false;
                return;
            }

            List<CameraData> cameraDataList = new List<CameraData>();

            TimelineAsset timeline = p.playableAsset as TimelineAsset;
            foreach (TrackAsset track in timeline.GetOutputTracks())
            {
                foreach (var timelineClip in track.GetClips())
                {
                    if (timelineClip.asset is StepCameraClip clip)
                    {
                        if (clip.cameraAsset != null)
                        {
                            if (clip.cameraAsset.VirtualCamera != null)
                            {
                                CameraData cameraData = new CameraData();
                                cameraData.AssetName = clip.cameraAsset.name;
                                cameraData.StartTime = (float)timelineClip.start;
                                cameraData.DurationTime = (float) timelineClip.duration;
                                cameraData.CameraAsset = clip.cameraAsset;
                                cameraData.TargetTransform = roleObj.transform;
                                cameraDataList.Add(cameraData);
                            }
                        }
                    }
                }
            }
            CameraEditorDebug.LogInfo("Editor Camera Init："+cameraDataList.Count);
            // CameraEditorEntity.CameraEditorManager.CameraCameraContextCtrl.OnInitLinkCameraControl(cameraDataList,null);
        }
        
        public void DriverLinkCameraProcess(float time,float frameTime)
        {
            if (_hasRole)
            {
                // CameraEditorEntity.CameraEditorManager.DriverLinkCameraProcess(time,frameTime);
            }
        }
    }
}