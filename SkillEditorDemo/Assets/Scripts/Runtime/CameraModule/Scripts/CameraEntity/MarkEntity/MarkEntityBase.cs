using Module.GameCore;
using Module.ObjectPool;
using UnityEngine;

namespace CameraModule.Runtime
{
    public abstract class MarkEntityBase : IClearObj
    {
        #region Property

        protected bool IsSlow;
        protected float Speed;
        protected float Weight;
        protected bool IsActive;
        protected Transform PosTargetTransform;
        protected Transform RotationTargetTransform;
        protected string GameObjectName;
        protected CameraModuleEntity CameraModuleEntity;

        #endregion
        
        
        public Transform MarkTransform;

        public abstract CameraMarkType GetCameraMarkType();
        
        public virtual void OnInitComponent(CameraMarkInfo cameraMarkInfo,CameraModuleEntity cameraModuleEntity)
        {
            Weight = cameraMarkInfo.CameraMarkData.RealWeight;
            IsSlow = cameraMarkInfo.CameraMarkData.IsSlow;
            Speed = cameraMarkInfo.CameraMarkData.Speed;
            PosTargetTransform = cameraMarkInfo.PosTarget;
            RotationTargetTransform = cameraMarkInfo.RotationTarget;
            GameObjectName = cameraMarkInfo.Name;
            CameraModuleEntity = cameraModuleEntity;
        }

        /// <summary>
        /// 获取Transform
        /// </summary>
        public virtual void HandleTransform()
        {
            Transform transform = CameraModuleEntity.CameraPoolComponent?.GetMarkEmptyGameObject(GameObjectName);
            if (transform!=null)
            {
                MarkTransform = transform;
            }
            else
            {
                CameraDebugger.LogError("Get Empty GameObject Is Null");
            }
            
        }

        /// <summary>
        /// 释放对象，放入对象池
        /// </summary>
        public virtual void OnRelease()
        {
            if (CameraModuleEntity.CameraEntityComponent!=null)
            {
                CameraModuleEntity.CameraPoolComponent.OnReleaseEmptyGameObject(MarkTransform);
                CameraModuleEntity.CameraEntityComponent.OnReleaseMarkEntity(this);
            }
            else
            {
                CameraDebugger.LogError("CameraEntityCtrl Is Null In MarkEntity");
            }
        }

        public virtual void OnUpdate(int updateTick,float frameTime)
        {
            
        }

        /// <summary>
        /// 清理数据
        /// </summary>
        public virtual void Clear()
        {
            Weight = 0;
            IsSlow = false;
            Speed = 0;
            PosTargetTransform = null;
            RotationTargetTransform = null;
            GameObjectName = "";
            MarkTransform = null;
        }
        
    }
}