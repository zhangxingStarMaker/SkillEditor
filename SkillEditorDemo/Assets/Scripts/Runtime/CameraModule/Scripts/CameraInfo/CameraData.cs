using Module.ObjectPool;
using UnityEngine;

namespace CameraModule.Runtime
{
    [System.Serializable]
    public class CameraData : IClearObj
    {
        public float StartTime;
        public float DurationTime;
        public float EndTime;
        public CameraAsset CameraAsset;
        public string AssetName;//资源路径
        public int Index;
        public Vector3 Pos;
        public Quaternion Rotation;
        public Transform TargetTransform;
        public string Name;

        /// <summary>
        /// 加载完成之后刷新资源
        /// </summary>
        public void RefreshData()
        {
            EndTime = StartTime + DurationTime;
        }

        public void Clear()
        {
            StartTime = 0;
            DurationTime = 0;
            EndTime = 0;
            CameraAsset = null;
            AssetName = "";
            Index = 0;
            Pos = Vector3.zero;
            Rotation = Quaternion.identity;
            TargetTransform = null;
        }
    }
}