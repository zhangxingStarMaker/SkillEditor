using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace CameraModule.Runtime
{
    [CreateAssetMenu(fileName = "camera_configure", menuName = "相机/相机配置表", order = 0)]
    public class CameraConfigureAsset : ScriptableObject
    {
        [HideInInspector]
        public List<CameraRuleConfigure> CameraRuleAssetList = new List<CameraRuleConfigure>();
        [HideInInspector]
        public List<CameraPathConfigure> CameraPathAssetList = new List<CameraPathConfigure>();
        [HideInInspector]
        public List<CameraGroupConfigure> CameraGroupAssetList = new List<CameraGroupConfigure>();
        
        //跳跃
        [HideInInspector]
        public List<CameraGroupConfigure> JumpNormalPreCameraGroupList = new List<CameraGroupConfigure>();
        [HideInInspector]
        public List<CameraGroupConfigure> JumpNormalAirCameraGroupList = new List<CameraGroupConfigure>();
        [HideInInspector]
        public List<CameraGroupConfigure> JumpBladePreCameraGroupList = new List<CameraGroupConfigure>();
        [HideInInspector]
        public List<CameraGroupConfigure> JumpBladeAirCameraGroupList = new List<CameraGroupConfigure>();
        [HideInInspector]
        public List<CameraGroupConfigure> JumpNormalMarkCameraGroupList = new List<CameraGroupConfigure>();
        [HideInInspector]
        public List<CameraGroupConfigure> JumpNormalFallCameraGroupList = new List<CameraGroupConfigure>();
        
        //旋转
        [HideInInspector]
        public List<CameraGroupConfigure> RotationCameraGroupList = new List<CameraGroupConfigure>();
        

        public List<CameraGroupInfo> CameraGroupInfoList = new List<CameraGroupInfo>();

        /// <summary>
        /// 动态添加CameraGroup
        /// </summary>
        /// <param name="cameraGroupInfo"></param>
        public void AddCameraGroupInfo(CameraGroupInfo cameraGroupInfo)
        {
            foreach (var groupInfo in CameraGroupInfoList)
            {
                if (groupInfo.AssetName.Equals(cameraGroupInfo.AssetName))
                {
                    CameraGroupInfoList.Remove(groupInfo);
                    break;
                }
            }
            
            CameraGroupInfoList.Add(cameraGroupInfo);
        }
    }
}