using Module.Battle.Camera;
using UnityEngine;

namespace CameraModule.Runtime
{
    [System.Serializable]
    public class CameraGroupItemAsset
    {
        public CameraAsset CameraAsset;
        public double StartTime;
        public double EndTime;
    }
}