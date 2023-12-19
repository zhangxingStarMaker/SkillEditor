using System.Collections.Generic;
using UnityEngine;

namespace CameraModule.Runtime
{
    public class CameraGmComponent : MonoBehaviour
    {
        public List<CameraData> CurrentCameraDataList;
        public List<CameraData> SourceCameraDataList = new List<CameraData>();
        public CameraGmAsset CameraGmAsset;

        public List<CameraModuleInfo> CameraModuleInfoList;
        
        public void SetCameraModuleInfoList(List<CameraModuleInfo> cameraModuleInfoList)
        {
            CameraModuleInfoList = cameraModuleInfoList;
        }
        
        public void SetCurrentCameraList(List<CameraData> cameraDataList)
        {
            CurrentCameraDataList = cameraDataList;
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
                SourceCameraDataList.Add(newCameraData);
            }
        }

        public void SaveCameraDataList()
        {
            if (CameraGmAsset!=null&& SourceCameraDataList !=null)
            {
                CameraGmAsset.RuntimeCameraDataList.Clear();
                foreach (var cameraData in SourceCameraDataList)
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
                    CameraGmAsset.RuntimeCameraDataList.Add(newCameraData);
                }

                if (CameraModuleInfoList!=null)
                {
                    CameraGmAsset.RuntimeCameraModuleInfoList.Clear();
                    foreach (var cameraModuleInfo in CameraModuleInfoList)
                    {
                        CameraModuleInfo newCameraData = new CameraModuleInfo();
                        newCameraData.AnimationKeyList = cameraModuleInfo.AnimationKeyList;
                        newCameraData.CameraKey = cameraModuleInfo.CameraKey;
                        CameraGmAsset.RuntimeCameraModuleInfoList.Add(newCameraData);
                    }
                }
            }
        }
    }
}


