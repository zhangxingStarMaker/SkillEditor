using System;
using System.Collections.Generic;
using CameraModule.Runtime;
using Cinemachine;
using Module.FrameBase;
using UnityEngine;

namespace Module.GameCore
{
    public class CameraModuleEntity : CoreEntity,ICoreEntityAwake,ICoreEntityUpdate,ICoreEntityPreload
    {

        private CameraAssetComponent _cameraAssetComponent;
        public CameraAssetComponent CameraAssetComponent => _cameraAssetComponent;
        

        private CameraControlComponent _cameraControlComponent;
        public CameraControlComponent CameraControlComponent => _cameraControlComponent;

        private CameraDataComponent _cameraDataComponent;
        public CameraDataComponent CameraDataComponent => _cameraDataComponent;

        private CameraEntityComponent _cameraEntityComponent;
        public CameraEntityComponent CameraEntityComponent => _cameraEntityComponent;

        private CameraPoolComponent _cameraPoolComponent;
        public CameraPoolComponent CameraPoolComponent => _cameraPoolComponent;

        private CameraTransformComponent _cameraTransformComponent;
        public CameraTransformComponent CameraTransformComponent => _cameraTransformComponent;
        
        private CameraRootNode _cameraRootNode;
        public CameraRootNode CameraRootNode => _cameraRootNode;
        
        /// <summary>
        /// 判断当前运行状态是否是编辑预览模式
        /// </summary>
        private bool _isEditorModel;
        public bool IsEditorModel => _isEditorModel;
        
        private CinemachineBrain _cineMachineBrain;
        public CinemachineBrain CineMachineBrain
        {
            get
            {
                if (_cineMachineBrain == null)
                {
                    if (Camera.main is not null) _cineMachineBrain = Camera.main.GetComponent<CinemachineBrain>();
                    if (_cineMachineBrain == null)
                    {
                        CameraDebugger.LogError("Main Camera No CinemachineBrain");
                    }
                }

                return _cineMachineBrain;
            }
        }


        public void OnAwake()
        {
            InitCameraRootNode();
            InitComponent();
        }

        private void InitComponent()
        {
            _cameraAssetComponent = AddComponent<CameraAssetComponent>();
            _cameraControlComponent = AddComponent<CameraControlComponent>();
            _cameraDataComponent = AddComponent<CameraDataComponent>();
            _cameraEntityComponent = AddComponent<CameraEntityComponent>();
            _cameraPoolComponent = AddComponent<CameraPoolComponent>();
            _cameraTransformComponent = AddComponent<CameraTransformComponent>();
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
        
        /// <summary>
        /// 手动初始化 主要用于编辑器模式下调用相机功能
        /// </summary>
        public void EditorManualInit()
        {
            
        }

        /// <summary>
        /// 初始化配置表相关
        /// </summary>
        public void InitConfigureAsset()
        {
            _cameraDataComponent.InitCameraConfigure();
        }

        /// <summary>
        /// 开始准备资源
        /// </summary>
        /// <param name="cameraDataList"></param>
        /// <param name="prepareCallBack"></param>
        public void OnPrepareCameraData(List<CameraData> cameraDataList,Action prepareCallBack = null)
        {
            _cameraDataComponent.OnPrepareCameraDataList(cameraDataList,prepareCallBack);
        }
        
        /// <summary>
        /// 单个相机准备资源
        /// </summary>
        /// <param name="cameraData"></param>
        /// <param name="loadCompleted"></param>
        public void OnPrepareSingleCameraControl(CameraData cameraData,Action loadCompleted = null)
        {
            _cameraDataComponent.OnPrepareSingleCameraData(cameraData,loadCompleted);
        }

        /// <summary>
        /// 初始化并且播放单个相机
        /// </summary>
        /// <param name="cameraData"></param>
        /// <param name="playCompleted"></param>
        public void OnInitAndPlaySingleCameraControl(CameraData cameraData, Action playCompleted = null)
        {
            _cameraControlComponent.OnInitAndPlaySingleCameraControl(cameraData,playCompleted);
        }

        /// <summary>
        /// 释放单个相机播放状态
        /// </summary>
        public void OnStopSingleCameraControl()
        {
            _cameraControlComponent.OnStopSingleCameraControl();
        }

        public CameraData GetCameraData()
        {
            return _cameraPoolComponent.GetCameraData();
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void OnReleaseCameraData(CameraData cameraData)
        {
            _cameraPoolComponent.OnReleaseCameraData(cameraData);
        }

        public CameraConfigureAsset GetCameraConfigure()
        {
            return _cameraDataComponent.GetCameraConfigure();
        }

        public CameraPathConfigure GetCameraPathConfigure(int id)
        {
            return _cameraDataComponent.GetCameraPathConfigure(id);
        }

        public CameraGroupConfigure GetCameraGroupConfigure(CameraTag tag)
        {
            return null;
        }

        public void SetCurrentCameraList(List<CameraData> cameraDataList)
        {
            _cameraRootNode.SetCurrentCameraList(cameraDataList);
        }

        public void SetCameraModuleInfoList(List<CameraModuleInfo> cameraModuleInfoList)
        {
            _cameraRootNode.SetCameraModuleInfoList(cameraModuleInfoList);
        }

        public CameraGroupInfo GetCameraGroupInfo(string key)
        {
            return _cameraDataComponent.GetCameraGroupInfo(key);
        }

        public void ReleaseCameraOverrider()
        {
            
        }

        /// <summary>
        /// 初始化数据，需要注意，调用此接口前需要调用OnPrepareCameraData进行预加载
        /// </summary>
        /// <param name="cameraDataList"></param>
        /// <param name="initCompleted"></param>
        public void OnInitLinkCameraControl(List<CameraData> cameraDataList,Action initCompleted = null)
        {
            _cameraControlComponent.OnInitLinkCameraControl(cameraDataList,initCompleted);
        }

        /// <summary>
        /// 停止连续相机播放，并且释放资源
        /// </summary>
        public void OnStopLinkCameraControl()
        {
            _cameraControlComponent.OnStopLinkProcess();
        }

        /// <summary>
        /// 驱动连续相机进程
        /// </summary>
        public void DriverLinkCameraProcess(float time,float frameTime)
        {
            _cameraControlComponent.DriverLinkCameraProcess(time,frameTime);
        }

        public void OnClear()
        {
            _cameraControlComponent.OnClear();
            _cameraEntityComponent.OnClear();
            _cameraPoolComponent.OnClear();
            _cameraDataComponent.OnClear();
            _cameraAssetComponent.OnClear();
        }
        
        public void OnPreload(Action continueCallback, IPreloadComponent preloadComponent, PreloadOpportunity opportunity)
        {
            
        }

        public void OnUpdate()
        {
            
        }
    }
}