using Cinemachine;
using Module.FrameBase;
using UnityEngine;

namespace CameraModule.Runtime
{
    public class CameraTransformComponent : CoreEntity,ICoreEntityAwake
    {
        private CameraRootNode _cameraRootNode;
        private CinemachineBrain _cinemachineBrain;
        
        public CameraRootNode CameraRootNode => _cameraRootNode;
        
        public CinemachineBrain CinemachineBrain
        {
            get
            {
                if (_cinemachineBrain == null)
                {
                    if (Camera.main is not null) _cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
                    if (_cinemachineBrain == null)
                    {
                        CameraDebugger.LogError("Main Camera No CinemachineBrain");
                    }
                }

                return _cinemachineBrain;
            }
        }
        
        public void OnAwake()
        {
            
        }

        public override void BeginInit()
        {
            base.BeginInit();
            InitCameraRootNode();
        }

        /// <summary>
        /// 初始化根节点信息
        /// </summary>
        private void InitCameraRootNode()
        {
            if (_cameraRootNode==null)
            {
                GameObject go = new GameObject("CameraRootNode");
                _cameraRootNode = go.AddComponent<CameraRootNode>();
                _cameraRootNode.OnInit(go);
            }  
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}