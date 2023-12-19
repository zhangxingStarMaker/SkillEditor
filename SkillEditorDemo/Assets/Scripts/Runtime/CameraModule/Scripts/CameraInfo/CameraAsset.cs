/*
* @Author: 白哉
* @Description: 相机的序列化数据类
* @Date: 2023年02月06日 星期一 14:02:36
* @Modify: 
*/


using UnityEngine;
using System.Collections.Generic;


namespace CameraModule.Runtime
{
    public enum CameraType
    {
        
        /// <summary>
        /// 普通相机
        /// </summary>
        [InspectorName("普通")]
        Normal,
        /// <summary>
        /// 美术相机
        /// </summary>
        [InspectorName("美术相机")]
        AnimationCamera,
    }
    [CreateAssetMenu(fileName = "cameraAsset", menuName = "camera/脚本相机", order = 0)]
    public class CameraAsset : ScriptableObject
    {
        [SerializeField]
        public CameraType CurCameraType = CameraType.Normal;
        public GameObject VirtualCamera;

        public List<CameraMarkData> CameraFollowMarks = new List<CameraMarkData>();
        public List<CameraMarkData> CameraLookMarks = new List<CameraMarkData>();
        public AnimationClip[] AnimationClip;
        public float TransitionTime = 0f;
    }

    
    [System.Serializable]
    public class CameraMarkData
    {
        public CameraTarget CameraMarkTarget = CameraTarget.None;
    
        public bool TimeOffsetControl = false;
        public float TimeOffset = 0;
        
        public Vector3 WorldPosition = Vector3.zero;
        public Quaternion WorldRotation = Quaternion.identity;

        public bool ActorOffsetControl = false;
        public CameraPlayerMarkOffsetType ActorOffsetType = CameraPlayerMarkOffsetType.World;
        public Vector3 ActorOffset = Vector3.zero;
        
        public bool PathOffsetControl = false;
        public CameraPathMarkOffsetType PathOffsetType = CameraPathMarkOffsetType.World;
        public Vector3 PathOffset = Vector3.zero;

        public float ScaleWeight = 1;
        [Range(0, 1)] public float RealWeight = 1;

        public bool IsSlow = false;
        public float Speed = 100;
        
        //--剧情
        public string DialogueTargetRoleName;
        //--剧情
    }
}