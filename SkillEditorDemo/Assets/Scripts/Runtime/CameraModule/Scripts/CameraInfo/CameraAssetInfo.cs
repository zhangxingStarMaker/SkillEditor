using System;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Battle.Camera
{
    [CreateAssetMenu(fileName = "CameraAssetInfo", menuName = "camera/脚本相机列表", order = 0)]
    public class CameraAssetInfo: ScriptableObject
    {
        [SerializeField]
        public List<OneCameraAsset> cameraInfoList = new List<OneCameraAsset>();
    }
    [Serializable]
    public class OneCameraAsset
    {
        [SerializeField]
        public double startTime;
        [SerializeField]
        public double duration;
        [SerializeField]
        public CameraAsset cameraAsset;

        public OneCameraAsset(double startTime, double duration, CameraAsset cameraAsset)
        {
            this.startTime = startTime;
            this.duration = duration;
            this.cameraAsset = cameraAsset;
        }

        public OneCameraAsset()
        {
            
        }
    }

    public class RecordOneCameraAsset:OneCameraAsset
    {
        public string CameraAssetAddress;

        public RecordOneCameraAsset(double startTime, double duration, CameraAsset cameraAsset) : base(startTime, duration, cameraAsset)
        {
        }

        public RecordOneCameraAsset()
        {
            
        }
    }
}