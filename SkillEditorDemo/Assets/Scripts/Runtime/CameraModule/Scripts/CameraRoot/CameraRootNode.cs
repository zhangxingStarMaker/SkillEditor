using System.Collections.Generic;
using UnityEngine;

namespace CameraModule.Runtime
{
    public class CameraRootNode : MonoBehaviour
    {
        private Transform _cameraRootTransform;
        private Transform _artCameraRootTransform;
        private Transform _normalCameraRootTransform;
        private Transform _normalCameraParentTransform;
        private Transform _cameraMarkRootTransform;
        private Transform _poolRootTransform;
        private CameraGmComponent _cameraGmComponent;

        public Transform CameraRootTransform => _cameraRootTransform;
        public Transform ArtCameraRootTransform => _artCameraRootTransform;

        public Transform NormalCameraRootTransform => _normalCameraRootTransform;

        public Transform CameraMarkRootTransform => _cameraMarkRootTransform;

        public Transform PoolRootTransform => _poolRootTransform;
        

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void SetCurrentCameraList(List<CameraData> cameraDataList)
        {
            _cameraGmComponent.SetCurrentCameraList(cameraDataList);
        }
        
        public void SetCameraModuleInfoList(List<CameraModuleInfo> cameraModuleInfoList)
        {
            _cameraGmComponent.SetCameraModuleInfoList(cameraModuleInfoList);
        }

        public void OnInit(GameObject go)
        {
            if (go != null)
            {
                _cameraRootTransform = go.transform;
                _cameraGmComponent = _cameraRootTransform.gameObject.AddComponent<CameraGmComponent>();
                InitArtCameraRoot();
                InitNormalCameraRoot();
                InitPoolRoot();
            }
            else
            {
                CameraDebugger.LogError("Camera Root Is Null");
            }
            
        }

        /// <summary>
        /// 初始化美术相机,动画相机
        /// </summary>
        private void InitArtCameraRoot()
        {
            if (_artCameraRootTransform == null)
            {
                _artCameraRootTransform = new GameObject("ArtCameraRoot").transform;
                if (_cameraRootTransform!=null)
                {
                    _artCameraRootTransform.SetParent(_cameraRootTransform);
                }
            }
        }

        /// <summary>
        /// 初始化常规相机,常规的用Cinemachine全部控制
        /// </summary>
        private void InitNormalCameraRoot()
        {
            if (_normalCameraParentTransform == null)
            {
                _normalCameraParentTransform = new GameObject("NormalCameraParent").transform;
                _normalCameraParentTransform.SetParent(_cameraRootTransform);
                if (_normalCameraRootTransform == null)
                {
                    _normalCameraRootTransform = new GameObject("NormalCameraRoot").transform;
                    _normalCameraRootTransform.SetParent(_normalCameraParentTransform);
                    if (_cameraMarkRootTransform == null)
                    {
                        _cameraMarkRootTransform = new GameObject("MarkRoot").transform;
                        _cameraMarkRootTransform.SetParent(_normalCameraParentTransform);
                    }
                }
            }
            
        }

        /// <summary>
        /// 初始化对象池父节点
        /// </summary>
        private void InitPoolRoot()
        {
            if (_poolRootTransform == null)
            {
                _poolRootTransform = new GameObject("CameraPoolRoot").transform;
                if (_cameraRootTransform!=null)
                {
                    _poolRootTransform.SetParent(_cameraRootTransform);
                }
            }
        }
    }
}