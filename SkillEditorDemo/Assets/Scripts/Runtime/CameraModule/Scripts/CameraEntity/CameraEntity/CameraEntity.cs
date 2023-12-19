using Cinemachine;
using Module.GameCore;
using Module.ObjectPool;
using Module.Utility;
using UnityEngine;
using CameraType = Module.Battle.Camera.CameraType;

namespace CameraModule.Runtime
{
    public abstract class CameraEntity : IClearObj
    {
        protected CameraData CameraData;
        public Transform CameraTransform;
        // protected CameraPoolCtrl CameraPoolCtrl;
        // protected CameraEntityCtrl CameraEntityCtrl;
        // protected CameraDataCtrl CameraDataCtrl;
        protected CinemachineVirtualCameraBase CinemachineCamera;

        protected CameraModuleEntity CameraModuleEntity;

        public virtual void OnInitComponent(CameraData cameraData,CameraModuleEntity cameraModuleEntity)
        {
            CameraData = cameraData;
            CameraModuleEntity = cameraModuleEntity;
        }

        public virtual CinemachineVirtualCameraBase GetCinemachineVirtualCamera()
        {
            return CinemachineCamera;
        }

        public virtual void ChangeVirtualCameraPriority(VirtualCameraPriority cameraPriority)
        {
            if (CinemachineCamera!=null)
            {
                CinemachineCamera.Priority = (int)cameraPriority;
            }
        }

        public virtual void InstanceObj()
        {
            if (CameraData.CameraAsset != null)
            {
                Transform tra = CameraModuleEntity.CameraPoolComponent.GetGameObject(CameraData);
                // Transform tra = CameraPoolCtrl.GetGameObject(CameraData);
                if (tra!=null)
                {
                    string name = "";
                    if (CameraData.CameraAsset.CurCameraType == CameraType.Normal)
                    {
                        name = "normalCamera_"+CameraData.Name+"_"+CameraData.Index;
                    }
                    else
                    {
                        name = "artCamera_"+CameraData.Name+"_"+CameraData.Index;
                    }
                    
                    // tra.name = name;
                    CameraTransform = tra;
                    CameraTransform.ResetTransform();
                    InitCinemachineComponent();
                }
            }
        }

        /// <summary>
        /// 获取Component
        /// </summary>
        protected virtual void InitCinemachineComponent()
        {
            CinemachineCamera = CameraTransform.GetComponent<CinemachineVirtualCameraBase>();
            if (CinemachineCamera != null)
            {
                ChangeVirtualCameraPriority(VirtualCameraPriority.None);
                CinemachineCamera.m_StandbyUpdate = CinemachineVirtualCameraBase.StandbyUpdateMode.Never;
            }
            else
            {
                CameraDebugger.LogError("CinemachineVirtualCamera Component Is Null");
            }
        }
        

        /// <summary>
        /// 资源加载完毕后处理Mark数据
        /// </summary>
        public virtual void HandleEntityInfo()
        {
            
        }
        
        public abstract CameraEntityType GetCameraType();
        public virtual void Clear()
        {
            // CameraPoolCtrl = null;
            // CameraEntityCtrl = null;
            // CameraDataCtrl = null;
            CinemachineCamera = null;
            CameraTransform = null;
        }

        public virtual void OnRelease()
        {
            if (CameraModuleEntity!=null)
            {
                ChangeVirtualCameraPriority(VirtualCameraPriority.None);
                //对象释放之前进行存储，后续使用
                CameraModuleEntity.CameraDataComponent.AddVirtualCamera(CameraData.AssetName,CameraTransform);
                CameraModuleEntity.CameraEntityComponent.OnReleaseCameraEntity(this);
            }
            else
            {
                CameraDebugger.LogError("CameraEntityCtrl Is Null In CameraEntity");
            }
        }

        public virtual void OnUpdate(float time,float frameTime)
        {
            
        }
    }
}