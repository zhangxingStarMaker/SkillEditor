using CameraModule.Runtime;
using Module.FrameBase;
using Module.GameCore;
using Module.ObjectPool;
using UnityEngine;

namespace CameraModule.Runtime
{
    public class CameraEntityComponent : CoreEntity,ICoreEntityAwake,ICoreEntityUpdate
    {
        protected CameraModuleEntity CameraModuleEntity;
        public void OnAwake()
        {
            CameraModuleEntity = this.Parent as CameraModuleEntity;
        }

        public void OnUpdate()
        {
            
        }
        
        #region API
        
        public CameraPrepareEntity GetCameraPrepareInfo()
        {
            return CommonObjectPool<CameraPrepareEntity>.Get(); 
        }
        
        public void OnReleaseCameraPrepareEntity(CameraPrepareEntity cameraPrepareEntity)
        {
            cameraPrepareEntity.ResetInfo();
            CommonObjectPool<CameraPrepareEntity>.Release(cameraPrepareEntity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ArtCameraEntity GetArtCameraEntity(CameraData cameraData)
        {
            ArtCameraEntity artCameraEntity = CommonObjectPool<ArtCameraEntity>.Get();
            artCameraEntity.OnInitComponent(cameraData,CameraModuleEntity);
            return artCameraEntity;
        }

        /// <summary>
        /// 获取CameraEntity,注意：获取后资源或许还没有加载完毕
        /// </summary>
        /// <param name="cameraData"></param>
        /// <returns></returns>
        public NormalCameraEntity GetNormalCameraEntity(CameraData cameraData)
        {
            NormalCameraEntity normalCameraEntity = CommonObjectPool<NormalCameraEntity>.Get();
            normalCameraEntity.OnInitComponent(cameraData,CameraModuleEntity);
            return normalCameraEntity;
        }

        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="cameraEntity"></param>
        public void OnReleaseCameraEntity(CameraEntity cameraEntity)
        {
            if (cameraEntity==null)
            {
                CameraDebugger.LogError("Release CameraEntity Is Null");
                return;
            }
            CameraEntityType cameraEntityType = cameraEntity.GetCameraType();
            switch (cameraEntityType)
            {
                case CameraEntityType.Normal:
                    CommonObjectPool<NormalCameraEntity>.Release(cameraEntity as NormalCameraEntity);
                    break;
                case CameraEntityType.Art:
                    CommonObjectPool<ArtCameraEntity>.Release(cameraEntity as ArtCameraEntity);
                    break;
            }
        }

        /// <summary>
        /// 获取Actor Entity
        /// </summary>
        /// <returns></returns>
        public MarkEntityBase GetMarkEntity(CameraMarkInfo cameraMarkInfo,Transform targetTransform)
        {
            MarkEntityBase markEntity = null;
            switch (cameraMarkInfo.CameraMarkData.CameraMarkTarget)
            {
                case CameraTarget.World:
                    if (targetTransform!=null)
                    {
                        cameraMarkInfo.RotationTarget = targetTransform;
                        cameraMarkInfo.PosTarget = targetTransform;
                    }
                    markEntity = CommonObjectPool<WorldMarkEntity>.Get();
                    break;
                case CameraTarget.Player_Root:
                    if (targetTransform!=null)
                    {
                        cameraMarkInfo.RotationTarget = targetTransform;
                        cameraMarkInfo.PosTarget = targetTransform.GetChild(0)?.Find("Root");
                    }
                    markEntity = CommonObjectPool<ActorMarkEntity>.Get();
                    break;
                case CameraTarget.Head:
                    if (targetTransform!=null)
                    {
                        cameraMarkInfo.RotationTarget = targetTransform;
                        cameraMarkInfo.PosTarget = targetTransform.GetChild(0)?.Find("Root/Bip001/Bip001_Pelvis/Bip001_Spine/Bip001_Spine1/Bip001_Spine2/Bip001_Neck/Bip001_Head");
                    }
                    markEntity = CommonObjectPool<ActorMarkEntity>.Get();
                    break;
                case CameraTarget.Chest:
                    if (targetTransform != null)
                    {
                        cameraMarkInfo.RotationTarget = targetTransform;
                        cameraMarkInfo.PosTarget = targetTransform.GetChild(0)?.Find("Root/Bip001/Bip001_Pelvis/Bip001_Spine/Bip001_Spine1/Bip001_Spine2");
                    }
                    markEntity = CommonObjectPool<ActorMarkEntity>.Get();
                    break;
                case CameraTarget.LeftFoot:
                    if (targetTransform!=null)
                    {
                        cameraMarkInfo.RotationTarget = targetTransform;
                        cameraMarkInfo.PosTarget = targetTransform.GetChild(0)?.Find("Root/Bip001/Bip001_Pelvis/Bip001_L_Thigh/Bip001_L_Calf/Bip001_L_Foot");
                    }
                    markEntity = CommonObjectPool<ActorMarkEntity>.Get();
                    break;
                case CameraTarget.RightFoot:
                    if (targetTransform!=null)
                    {
                        cameraMarkInfo.RotationTarget = targetTransform;
                        cameraMarkInfo.PosTarget = targetTransform.GetChild(0)?.Find("Root/Bip001/Bip001_Pelvis/Bip001_R_Thigh/Bip001_R_Calf/Bip001_R_Foot");
                    }
                    markEntity = CommonObjectPool<ActorMarkEntity>.Get();
                    break;
                case CameraTarget.Path:
                    if (targetTransform!=null)
                    {
                        cameraMarkInfo.RotationTarget = targetTransform;
                        cameraMarkInfo.PosTarget = targetTransform;
                    }
                    markEntity = CommonObjectPool<PathMarkEntity>.Get();
                    break;
            }
            
            markEntity?.OnInitComponent(cameraMarkInfo,CameraModuleEntity);
            
            return markEntity;
        }

        /// <summary>
        /// 释放MarkEntity
        /// </summary>
        /// <param name="markEntityBase"></param>
        public void OnReleaseMarkEntity(MarkEntityBase markEntityBase)
        {
            if (markEntityBase==null)
            {
                CameraDebugger.LogError("Release MarkEntityBase Is Null");
                return;
            }
            
            CameraMarkType cameraMarkType = markEntityBase.GetCameraMarkType();
            
            switch (cameraMarkType)
            {
                case CameraMarkType.Actor:
                    CommonObjectPool<ActorMarkEntity>.Release(markEntityBase as ActorMarkEntity);
                    break;
                case CameraMarkType.Path:
                    CommonObjectPool<PathMarkEntity>.Release(markEntityBase as PathMarkEntity);
                    break;
                case CameraMarkType.World:
                    CommonObjectPool<WorldMarkEntity>.Release(markEntityBase as WorldMarkEntity);
                    break;
                case CameraMarkType.None:
                    CameraDebugger.LogError("Camera Mark Has Not Type");
                    break;
            }
        }
        
        public void OnClear()
        {
            CommonObjectPool<ArtCameraEntity>.Clear();
            CommonObjectPool<NormalCameraEntity>.Clear();
            CommonObjectPool<ActorMarkEntity>.Clear();
            CommonObjectPool<WorldMarkEntity>.Clear();
            CommonObjectPool<PathMarkEntity>.Clear();
            CommonObjectPool<CameraPrepareEntity>.Clear();
        }

        #endregion

        #region Pool List Info
        
        /// <summary>
        /// 初始化对象池信息
        /// </summary>
        private void InitPoolList()
        {
            CommonObjectPool<ArtCameraEntity>.Init(CreateCameraEntity<ArtCameraEntity>);
            CommonObjectPool<NormalCameraEntity>.Init(CreateCameraEntity<NormalCameraEntity>);
            CommonObjectPool<ActorMarkEntity>.Init(CreateMarkEntity<ActorMarkEntity>);
            CommonObjectPool<WorldMarkEntity>.Init(CreateMarkEntity<WorldMarkEntity>);
            CommonObjectPool<PathMarkEntity>.Init(CreateMarkEntity<PathMarkEntity>);
            CommonObjectPool<CameraPrepareEntity>.Init(CreateCameraPrepareEntity);
        }
        
        private CameraPrepareEntity CreateCameraPrepareEntity()
        {
            return new CameraPrepareEntity();
        }
        
        /// <summary>
        /// 创建CameraEntity
        /// </summary>
        /// <typeparam name="TCameraEntity"></typeparam>
        /// <returns></returns>
        private TCameraEntity CreateCameraEntity<TCameraEntity>() where TCameraEntity : CameraEntity, new()
        {
            return new TCameraEntity();
        }
        
        /// <summary>
        /// 创建MarkEntity
        /// </summary>
        /// <typeparam name="TMarkEntity"></typeparam>
        /// <returns></returns>
        private TMarkEntity CreateMarkEntity<TMarkEntity>() where TMarkEntity : MarkEntityBase, new()
        {
            return new TMarkEntity();
        }
        
        #endregion
    }
}