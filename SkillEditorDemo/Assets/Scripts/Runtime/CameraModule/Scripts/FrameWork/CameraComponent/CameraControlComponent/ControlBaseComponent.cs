using System;
using Cinemachine;
using Module.FrameBase;
using Module.GameCore;
using Module.ObjectPool;

namespace CameraModule.Runtime
{
    public abstract class ControlBaseComponent : CoreEntity,ICoreEntityAwake,ICoreEntityUpdate,IClearObj
    {
        protected CinemachineBrain CinemachineBrain;
        protected float StartTime;
        protected CameraModuleEntity CameraModuleEntity;
        
        public CameraWorkState CameraWorkState { get; protected set; }
        
        public void OnAwake()
        {
            
        }
        
        public virtual void OnInit(CinemachineBrain cinemachineBrain,CameraModuleEntity cameraModuleEntity)
        {
            CameraWorkState = CameraWorkState.None;
            CinemachineBrain = cinemachineBrain;
            CameraModuleEntity = cameraModuleEntity;
        }

        /// <summary>
        /// 修改当前状态
        /// </summary>
        /// <param name="cameraWorkState"></param>
        public virtual void ChangeCameraWorkState(CameraWorkState cameraWorkState)
        {
            CameraWorkState = cameraWorkState;
        }
        
        /// <summary>
        /// 准备资源,只要调用就会进入准备资源状态，不检验资源的正确性
        /// </summary>
        public virtual void OnInitialization()
        {
            
        }

        protected virtual void CheckInitializationState()
        {
            
        }

        public virtual void OnPlay(float startTime,Action playEndCallBack = null)
        {
            StartTime = startTime;
        }

        public virtual void OnStop()
        {
            
        }
        
        /// <summary>
        /// 轮训
        /// </summary>
        public virtual void OnTick(int updateTicks)
        {
            if (CameraWorkState == CameraWorkState.Initialization)
            {
                CheckInitializationState();
            }
        }

        public virtual void Clear()
        {
            CameraWorkState = CameraWorkState.Free;
            CinemachineBrain = null;
            CameraModuleEntity = null;
        }

        public void OnUpdate()
        {
            
        }

        /// <summary>
        /// 释放
        /// </summary>
        public virtual void OnRelease()
        {
            
        }
        
    }
}