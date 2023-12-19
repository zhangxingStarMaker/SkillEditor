using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Module.Battle.Camera;
using Module.FrameBase;
using Moudule.ETTaskCore;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CameraModule.Runtime
{
    public class CameraAssetComponent : CoreEntity,ICoreEntityAwake
    {
        //todo 资源加载需要接外部接口
        private readonly Dictionary<string, uint> _loadAssetMap = new Dictionary<string, uint>();
        
        public void OnAwake()
        {
            CameraDebugger.Log("Camera Asset Init");
        }

        #region API

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="assetPath"></param>
        /// <param name="loadObjCallback"></param>
        public void LoadAssetASyn(string assetPath, Action<string, uint, UnityEngine.Object> loadObjCallback)
        {
            if (!_loadAssetMap.ContainsKey(assetPath))
            {
                // uint loadId = ResLoadFacade.Instance.LoadAssetASyn(assetPath, loadObjCallback);
                uint loadId = 1;
                if (loadId == 0)
                {
                    CameraDebugger.LogError("Load Id Is 0");
                }
                _loadAssetMap.Add(assetPath,loadId);
            }
            
        }
        
        public async ETTask<Object> LoadAssetASyn(string assetPath)
        {
            if (!_loadAssetMap.ContainsKey(assetPath))
            {
                // var resResult = await ResLoadFacade.Instance.LoadAssetASynWithTask<Object>(assetPath);
                //
                // if (resResult.Obj != null)
                // {
                //     _loadAssetMap.Add(assetPath,resResult.LoadId);
                //     return resResult.Obj;
                // }
            }

            return null;
        }

        public void UnLoadAsset(string assetName)
        {
            if (_loadAssetMap.ContainsKey(assetName))
            {
                // ResLoadFacade.Instance.UnLoadAssetByLoadId(_loadAssetMap[assetName]);
            }
            else
            {
                CameraDebugger.LogError("Load Map Has Not This AssetName"+assetName);
            }
        }
        
        /// <summary>
        /// 清理所有相机资源
        /// </summary>
        public void UnLoadMapAsset()
        {
            foreach (var loadId in _loadAssetMap.Values)
            {
                // ResLoadFacade.Instance.UnLoadAssetByLoadId(loadId);
            }
            _loadAssetMap.Clear();
        }

        /// <summary>
        /// 加载CameraAsset
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="callBack"></param>
        public void LoadCameraAssetASyn(string assetName,Action<CameraAsset> callBack)
        {
            LoadAssetASyn(assetName, (asset, loadId, obj) =>
            {
                CameraAsset cameraAsset = obj as CameraAsset;
                if (cameraAsset != null)
                {
                    callBack?.Invoke(cameraAsset);
                }
                else
                {
                    CameraDebugger.LogError("Camera Asset Is Null" + assetName);
                }
                
            });
            
            
        }
        
        /// <summary>
        /// 加载相机资源
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public async ETTask<CameraAsset> LoadCameraAssetASyn(string assetName)
        {
            var obj =  await LoadAssetASyn(assetName);
            return obj as CameraAsset;
        }
        
        public void OnClear()
        {
            _loadAssetMap.Clear();
        }

        public override void Dispose()
        {
            base.Dispose();
            _loadAssetMap.Clear();
        }

        #endregion
    }
}