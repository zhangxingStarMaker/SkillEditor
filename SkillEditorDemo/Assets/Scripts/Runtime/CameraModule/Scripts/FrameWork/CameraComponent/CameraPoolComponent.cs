using System;
using CameraModule.Runtime;
using Module.FrameBase;
using Module.GameCore;
using Module.ObjectPool;
using Module.Utility;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CameraModule.Runtime
{
    public class CameraPoolComponent : CoreEntity,ICoreEntityAwake
    {
        private CameraModuleEntity _cameraModuleEntity;
        public void OnAwake()
        {
            _cameraModuleEntity = this.Parent as CameraModuleEntity;
            CommonObjectPool<Transform>.Init(CreateTransform);
            CommonObjectPool<CameraMarkInfo>.Init(CreateClass<CameraMarkInfo>);
            CommonObjectPool<SingleCameraComponent>.Init(CreateClass<SingleCameraComponent>);
            CommonObjectPool<DampingRecorderData>.Init(CreateClass<DampingRecorderData>);
            CommonObjectPool<CameraData>.Init(CreateClass<CameraData>);
        }
        
        private Transform CreateTransform()
        {
            GameObject go = new GameObject();
            return go.transform;
        }

        private T CreateClass<T>() where T : class, new()
        {
            return new T();
        }

        #region API

        public CameraData GetCameraData()
        {
            return CommonObjectPool<CameraData>.Get();
        }
        
        public void OnReleaseCameraData(CameraData cameraData)
        {
            CommonObjectPool<CameraData>.Release(cameraData);
        }

        public SingleCameraComponent GetSingleCameraComponent()
        {
            return CommonObjectPool<SingleCameraComponent>.Get();
        }

        /// <summary>
        /// 释放单个CameraControl
        /// </summary>
        /// <param name="singleCameraComponent"></param>
        public void OnReleaseSingleCameraControl(SingleCameraComponent singleCameraComponent)
        {
            if (singleCameraComponent!=null)
            {
                CommonObjectPool<SingleCameraComponent>.Release(singleCameraComponent);
            }
            else
            {
                CameraDebugger.LogError("Release SingleCameraControl Is Null");
            }
            
        }
        
        public DampingRecorderData GetDampingRecorderData()
        {
            return CommonObjectPool<DampingRecorderData>.Get();
        }

        public void OnReleaseDampingRecorderData(DampingRecorderData dampingRecorderData)
        {
            dampingRecorderData.ResetData();
            CommonObjectPool<DampingRecorderData>.Release(dampingRecorderData);
        }

        /// <summary>
        /// 获取CameraMarkInfo
        /// </summary>
        /// <param name="cameraMarkData"></param>
        /// <returns></returns>
        public CameraMarkInfo GetCameraMarkInfo(CameraMarkData cameraMarkData)
        {
            CameraMarkInfo cameraMarkInfo = CommonObjectPool<CameraMarkInfo>.Get();
            cameraMarkInfo.CameraMarkData = cameraMarkData;
            return cameraMarkInfo;
        }

        /// <summary>
        /// 获取空的GameObject物体
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private Transform GetEmptyGameObject(string name)
        {
            Transform go = CommonObjectPool<Transform>.Get();
            go.name = name;
            return go;
        }
        
        /// <summary>
        /// 获取空的GameObject物体
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Transform GetMarkEmptyGameObject(string name)
        {
            Transform go = GetEmptyGameObject(name);
            go.transform.SetParent(_cameraModuleEntity.CameraRootNode.CameraMarkRootTransform);
            go.transform.ResetTransform();
            return go;
        }

        /// <summary>
        /// 释放对象
        /// </summary>
        /// <param name="transform"></param>
        public void OnReleaseEmptyGameObject(Transform transform)
        {
            if (transform!=null)
            {
                transform.SetParent(_cameraModuleEntity.CameraRootNode.PoolRootTransform);
                transform.ResetTransform();
                CommonObjectPool<Transform>.Release(transform);
            }
        }
        
        /// <summary>
        /// 获取空的GameObject物体
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Transform GetArtEntityParentEmptyGameObject(string name)
        {
            Transform transform = GetEmptyGameObject(name);
            transform.SetParent(_cameraModuleEntity.CameraRootNode.ArtCameraRootTransform);
            transform.ResetTransform();
            return transform;
        }

        /// <summary>
        /// 根据资源实例化对象
        /// </summary>
        /// <param name="cameraData"></param>
        /// <returns></returns>
        public Transform GetGameObject(CameraData cameraData)
        {
            Transform tra = _cameraModuleEntity.CameraDataComponent.GetVirtualCamera(cameraData.AssetName);
            if (tra == null)
            {
                tra = Object.Instantiate(cameraData.CameraAsset.VirtualCamera,
                    _cameraModuleEntity.CameraRootNode.NormalCameraRootTransform).transform;
            }
            else
            {
                tra.SetParent(_cameraModuleEntity.CameraRootNode.NormalCameraRootTransform);
            }

            return tra;
        }

        /// <summary>
        /// 获取Asset资源
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="callBack"></param>
        /// <returns></returns>
        public void GetCameraAsset(string assetName,Action<CameraAsset> callBack)
        {
            if (_cameraModuleEntity.CameraDataComponent!=null && _cameraModuleEntity.CameraDataComponent.GetCameraAsset(assetName) != null)
            {
                CameraAsset cameraAsset = _cameraModuleEntity.CameraDataComponent.GetCameraAsset(assetName);
                callBack?.Invoke(cameraAsset);
            }
            else
            {
                _cameraModuleEntity.CameraAssetComponent.LoadCameraAssetASyn(assetName, (cameraAsset) =>
                {
                    _cameraModuleEntity.CameraDataComponent.SetCameraAsset(assetName,cameraAsset);
                    callBack?.Invoke(cameraAsset);
                });
            }
        }

        /// <summary>
        /// 相机数据
        /// </summary>
        /// <param name="cameraData"></param>
        public void PrepareCameraData(CameraData cameraData)
        {
            if (_cameraModuleEntity.CameraDataComponent.GetCameraAsset(cameraData.AssetName) != null)
            {
                CameraAsset cameraAsset = _cameraModuleEntity.CameraDataComponent.GetCameraAsset(cameraData.AssetName);
                cameraData.CameraAsset = cameraAsset;
            }
            else
            {
                _cameraModuleEntity.CameraAssetComponent.LoadCameraAssetASyn(cameraData.AssetName, (cameraAsset) =>
                {
                    _cameraModuleEntity.CameraDataComponent.SetCameraAsset(cameraData.AssetName,cameraAsset);
                    cameraData.CameraAsset = cameraAsset;
                });
            }
        }
        
        public void OnClear()
        {
            CommonObjectPool<CameraMarkInfo>.Clear();
            CommonObjectPool<SingleCameraComponent>.Clear();
            CommonObjectPool<DampingRecorderData>.Clear();
            CommonObjectPool<CameraData>.Clear();
        }
        
        #endregion
    }
}