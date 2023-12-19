using System;
using System.Collections.Generic;
using Module.FrameBase;
using Module.GameCore;
using UnityEngine;

namespace CameraModule.Runtime
{
    public class CameraControlComponent : CoreEntity,ICoreEntityAwake,ICoreEntityUpdate
    {
        #region private property
        
        private SingleCameraComponent _currentSingleCameraComponent;
        private SingleCameraComponent _singleCameraComponent;
        private LinkCameraComponent _linkCameraComponent;
        private CameraModuleEntity _cameraModuleEntity;
        
        private CameraPlayMode _cameraPlayMode;

        #endregion
        
        
        public void OnAwake()
        {
            _cameraModuleEntity = this.Parent as CameraModuleEntity;
            CameraDebugger.Log("Camera Control Init");
            _cameraPlayMode = CameraPlayMode.None;
            _linkCameraComponent = AddComponent<LinkCameraComponent>();
            _linkCameraComponent.OnInit(_cameraModuleEntity.CineMachineBrain,_cameraModuleEntity);
            _singleCameraComponent = AddComponent<SingleCameraComponent>();
            _singleCameraComponent.OnInit(_cameraModuleEntity.CineMachineBrain,_cameraModuleEntity);
        }

        public void OnUpdate()
        {
            if (_cameraPlayMode == CameraPlayMode.Link)
            {
                _linkCameraComponent?.OnTick(1);
            }
            else if (_cameraPlayMode == CameraPlayMode.Single)
            {
                _currentSingleCameraComponent?.OnTick(1);
            }
        }
        
        /// <summary>
        /// 开始准备连续相机资源
        /// </summary>
        /// <param name="cameraDataList"></param>
        /// <param name="initCallBack"></param>
        public void OnInitLinkCameraControl(List<CameraData> cameraDataList,Action initCallBack)
        {
            switch (_cameraPlayMode)
            {
                case CameraPlayMode.Link:
                    //如果上一次也是Link相机，安全起见对上一次的数据进行清理
                    OnStopLinkProcess();
                    break;
                case CameraPlayMode.Single:
                    //如果上一次是Single相机，安全起见对上一次的数据进行清理
                    OnStopSingleCameraControl();
                    break;
            }
            ChangePlayMode(CameraPlayMode.Link);
            _linkCameraComponent.SetData(cameraDataList,initCallBack);
            _linkCameraComponent.OnInitialization();
        }

        public void OnInitAndPlaySingleCameraControl(CameraData cameraData, Action playCompleted)
        {
            switch (_cameraPlayMode)
            {
                case CameraPlayMode.Link:
                    //如果上一次也是Link相机，安全起见对上一次的数据进行清理
                    OnStopLinkProcess();
                    break;
                case CameraPlayMode.Single:
                    //如果上一次是Single相机，安全起见对上一次的数据进行清理
                    OnStopSingleCameraControl();
                    break;
            }
            ChangePlayMode(CameraPlayMode.Single);
            _currentSingleCameraComponent = _cameraModuleEntity.CameraPoolComponent.GetSingleCameraComponent();
            _currentSingleCameraComponent.OnInit(_cameraModuleEntity.CineMachineBrain,_cameraModuleEntity);
            cameraData.Index = -1;
            _currentSingleCameraComponent.SetData(cameraData);
            _currentSingleCameraComponent.OnInitialization();
            _currentSingleCameraComponent.OnPlay(Time.realtimeSinceStartup);
        }

        public void OnStopLinkProcess()
        {
            ChangePlayMode(CameraPlayMode.Free);
            _linkCameraComponent.OnStop();
        }
        
        public void OnStopSingleCameraControl()
        {
            ChangePlayMode(CameraPlayMode.Free);
            if (_currentSingleCameraComponent!=null)
            {
                _currentSingleCameraComponent.OnRelease();
                _currentSingleCameraComponent = null;
            }
            else
            {
                CameraDebugger.Log("Current SingleCameraControl Is Released");
            }
        }
        
        public void DriverLinkCameraProcess(float time,float frameTime)
        {
            _linkCameraComponent.DriverLinkCameraDataProcess(time,frameTime);
        }

        private void ChangePlayMode(CameraPlayMode cameraPlayMode)
        {
            _cameraPlayMode = cameraPlayMode;
        }

        public void OnClear()
        {
            OnStopSingleCameraControl();
            OnStopLinkProcess();
        }
    }
}