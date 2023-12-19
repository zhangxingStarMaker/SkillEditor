using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CameraModule.Runtime
{
    [CreateAssetMenu(fileName = "camera_group", menuName = "相机/相机组配置", order = 0)]
    public class CameraGroupAsset : ScriptableObject
    {
        public List<CameraGroupItemAsset> CameraGroupItemAssetList = new List<CameraGroupItemAsset>();
        public double StartTime;
        public double EndTime;
    }
}