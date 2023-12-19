using System;
using System.Collections.Generic;
using Module.GameCore;

namespace CameraModule.Runtime
{
    public class CameraPrepareEntity
    {
        protected List<CameraData> CameraDataList;
        private List<string> _loadAssetNameList = new List<string>();
        private Action _callBack;
        private CameraModuleEntity _cameraModuleEntity;

        public void OnInitComponent(List<CameraData> cameraDataList,Action callBack,CameraModuleEntity cameraModuleEntity)
        {
            CameraDataList = cameraDataList;
            _callBack = callBack;
            _cameraModuleEntity = cameraModuleEntity;
        }

        public virtual void OnPrepareAsset()
        {
            if (CameraDataList!=null)
            {
                foreach (var cameraData in CameraDataList)
                {
                    if (string.IsNullOrEmpty(cameraData.AssetName)==false &&!_loadAssetNameList.Contains(cameraData.AssetName))
                    {
                        if (!_cameraModuleEntity.CameraDataComponent.HasCameraAsset(cameraData.AssetName))
                        {
                            _cameraModuleEntity.CameraAssetComponent.LoadCameraAssetASyn(cameraData.AssetName, (cameraAsset) =>
                            {
                                _cameraModuleEntity.CameraDataComponent.SetCameraAsset(cameraData.AssetName,cameraAsset);
                            });
                            _loadAssetNameList.Add(cameraData.AssetName);
                        }
                    }
                }
            }
        }

        public bool CheckOnPrepared()
        {
            bool isPrepared = false;
            if (CameraDataList!=null)
            {
                foreach (var cameraData in CameraDataList)
                {
                    if (_cameraModuleEntity.CameraDataComponent.GetCameraAsset(cameraData.AssetName))
                    {
                        isPrepared = true;
                    }
                    else
                    {
                        isPrepared = false;
                        break;
                    }
                }
            }
            else
            {
                isPrepared = true;
            }

            return isPrepared;
        }

        /// <summary>
        /// 执行回调
        /// </summary>
        public void OnPreparedCallBack()
        {
            _callBack?.Invoke();
        }

        public void OnRelease()
        {
            _cameraModuleEntity.CameraEntityComponent?.OnReleaseCameraPrepareEntity(this);
        }

        public void ResetInfo()
        {
            CameraDataList = null;
            _callBack = null;
            _loadAssetNameList.Clear();
        }
    }
}