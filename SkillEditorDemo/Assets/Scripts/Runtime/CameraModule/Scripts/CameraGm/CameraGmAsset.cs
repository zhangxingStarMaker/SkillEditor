using System.Collections.Generic;
using UnityEngine;

namespace CameraModule.Runtime
{
    [CreateAssetMenu(fileName = "camera_configure", menuName = "相机/相机GM配置", order = 0)]
    public class CameraGmAsset : ScriptableObject
    {
        public List<CameraData> RuntimeCameraDataList = new List<CameraData>();
        public List<CameraData> EditorCameraDataList = new List<CameraData>();

        public List<CameraModuleInfo> RuntimeCameraModuleInfoList = new List<CameraModuleInfo>();
        public List<CameraModuleInfo> EditorCameraModuleInfoList = new List<CameraModuleInfo>();
    }
}