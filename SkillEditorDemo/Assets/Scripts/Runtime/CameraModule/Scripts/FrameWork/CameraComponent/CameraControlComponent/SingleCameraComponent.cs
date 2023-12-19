using System;
using Cinemachine;
using Module.GameCore;
using UnityEngine;

namespace CameraModule.Runtime
{
    public class SingleCameraComponent : ControlBaseComponent
    {
        public CameraData CameraData;
        private CameraEntity _cameraEntity;

        public override void OnInit(CinemachineBrain cinemachineBrain,CameraModuleEntity cameraModuleEntity)
        {
            base.OnInit(cinemachineBrain,cameraModuleEntity);
        }

        public override void OnTick(int updateTicks)
        {
            base.OnTick(updateTicks);
            UpdateSingleTime();
        }

        //用于连续相机刷新时间
        public void DriverSingleCameraProcess(float time,float frameTime)
        {
            _cameraEntity?.OnUpdate(time,frameTime);
        }

        /// <summary>
        /// 单个刷新
        /// </summary>
        public void UpdateSingleTime()
        {
            if (CameraWorkState == CameraWorkState.Working)
            {
                float time = Time.realtimeSinceStartup - StartTime;
                _cameraEntity?.OnUpdate(time,Time.deltaTime);
            }
        }

        /// <summary>
        /// 检查是否已经准备完毕
        /// </summary>
        protected override void CheckInitializationState()
        {
            if (_cameraEntity != null)
            {
                if (_cameraEntity.CameraTransform != null)
                {
                    CameraWorkState = CameraWorkState.InitializationCompleted;
                }
                else
                {
                    CameraDebugger.LogError("Camera Entity Is Null,Please Check"+ CameraData.AssetName);
                    CameraWorkState = CameraWorkState.Error;
                }
            }
        }

        /// <summary>
        /// 开始播放
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="playEndCallBack"></param>
        public override void OnPlay(float startTime,Action playEndCallBack = null)
        {
            base.OnPlay(startTime,playEndCallBack);
            ChangeCameraWorkState(CameraWorkState.Working);
        }

        public void SetData(CameraData cameraData)
        {
            CameraData = cameraData;
        }

        public CinemachineVirtualCameraBase GetCinemachineVirtualCamera()
        {
            return _cameraEntity?.GetCinemachineVirtualCamera();
        }

        public override void ChangeCameraWorkState(CameraWorkState cameraWorkState)
        {
            base.ChangeCameraWorkState(cameraWorkState);
            switch (cameraWorkState)
            {
                case CameraWorkState.Working:
                    _cameraEntity?.ChangeVirtualCameraPriority(VirtualCameraPriority.ScriptCamera);
                    break;
                case CameraWorkState.Free:
                    _cameraEntity?.ChangeVirtualCameraPriority(VirtualCameraPriority.DefaultCamera);
                    break;
            }
        }

        public CameraEntityType GetCameraEntityType()
        {
            if (_cameraEntity!=null)
            {
                return _cameraEntity.GetCameraType();
            }

            return CameraEntityType.None;
        }

        public override void OnInitialization()
        {
            base.OnInitialization();
            CameraWorkState = CameraWorkState.Initialization;//进入准备资源状态
            
            if (!string.IsNullOrEmpty(CameraData.AssetName))
            {
                if (CameraData.CameraAsset == null)
                {
                    CameraModuleEntity.CameraPoolComponent.PrepareCameraData(CameraData);
                }
                CreateEntity();
            }
            else
            {
                CameraDebugger.LogError("Camera Data Is Null");
            }
        }
        
        /// <summary>
        /// 加载回调
        /// </summary>
        private void CreateEntity()
        {
            CameraAsset cameraAsset = CameraData.CameraAsset;
            if (cameraAsset!=null)
            {
                CameraData.CameraAsset = cameraAsset;
                if (cameraAsset.CurCameraType == CameraType.Normal)
                {
                    _cameraEntity = CameraModuleEntity.CameraEntityComponent.GetNormalCameraEntity(CameraData);
                }
                else if (cameraAsset.CurCameraType == CameraType.AnimationCamera)
                {
                    _cameraEntity = CameraModuleEntity.CameraEntityComponent.GetArtCameraEntity(CameraData);
                }
                _cameraEntity?.InstanceObj();
                _cameraEntity?.HandleEntityInfo();
            }
            else
            {
                CameraDebugger.LogError("CameraAsset Load Failed " + CameraData.AssetName);
            }
        }

        public override void OnRelease()
        {
            base.OnRelease();
            _cameraEntity.OnRelease();
            CameraModuleEntity.CameraPoolComponent.OnReleaseSingleCameraControl(this);
            ChangeCameraWorkState(CameraWorkState.Ended);
        }

        public override void Clear()
        {
            base.Clear();
            _cameraEntity = null;
            ChangeCameraWorkState(CameraWorkState.Free);
        }
    }
}