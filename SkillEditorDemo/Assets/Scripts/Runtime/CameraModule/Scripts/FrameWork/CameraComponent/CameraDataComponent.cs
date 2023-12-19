using System;
using System.Collections.Generic;
using Module.Battle.Camera;
using Module.FrameBase;
using Module.GameCore;
using Module.Utility;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace CameraModule.Runtime
{
    public class CameraDataComponent : CoreEntity,ICoreEntityAwake
    {
        public Dictionary<string, CameraAsset> CameraAssetDic = new Dictionary<string, CameraAsset>(10);
        public Dictionary<string, Queue<Transform>> VirtualCameraObjDic = new Dictionary<string, Queue<Transform>>(10);
        private Dictionary<string, CameraGroupInfo> _cameraGroupInfoDic = new Dictionary<string, CameraGroupInfo>();
        private List<CameraPrepareEntity> _cameraPrepareInfoList = new List<CameraPrepareEntity>();
        
        private CameraConfigureAsset _cameraConfigureAsset;
        private Dictionary<int, CameraPathConfigure> _cameraPathConfigureDic;

        private Dictionary<CameraTag, List<CameraGroupConfigure>> _cameraGroupConfigureDic =
            new Dictionary<CameraTag, List<CameraGroupConfigure>>();

        private CameraModuleEntity _cameraModuleEntity;
        
        public void OnAwake()
        {
            // this.Parent.
            _cameraModuleEntity = this.Parent as CameraModuleEntity;
        }
        
        #region CameraConfigure
        
        public async void InitCameraConfigure()
        {
            if (_cameraConfigureAsset == null)
            {
                Object resResult = await _cameraModuleEntity.CameraAssetComponent.LoadAssetASyn(CameraConstDefines.CameraConfigurePath);
                if (resResult!=null)
                {
                    LoadCameraConfigureCallBack(resResult);
                }
            }
        }
        
        private void LoadCameraConfigureCallBack(Object obj)
        {
            if (obj!=null)
            {
                _cameraConfigureAsset = obj as CameraConfigureAsset;
                if (_cameraConfigureAsset is not null)
                {
                    _cameraPathConfigureDic = new Dictionary<int, CameraPathConfigure>(_cameraConfigureAsset.CameraPathAssetList.Count);
                    foreach (var cameraPathConfigure in _cameraConfigureAsset.CameraPathAssetList)
                    {
                        _cameraPathConfigureDic.Add(cameraPathConfigure.ID,cameraPathConfigure);
                    }

                    foreach (var cameraGroupInfo in _cameraConfigureAsset.CameraGroupInfoList)
                    {
                        if (!_cameraGroupInfoDic.ContainsKey(cameraGroupInfo.AssetName))
                        {
                            _cameraGroupInfoDic.Add(cameraGroupInfo.AssetName,cameraGroupInfo);   
                        }
                    }
                    AddCameraGroupTag(CameraTag.JumpNormalAir, _cameraConfigureAsset.JumpNormalAirCameraGroupList);
                    AddCameraGroupTag(CameraTag.JumpNormalPre, _cameraConfigureAsset.JumpNormalPreCameraGroupList);
                    AddCameraGroupTag(CameraTag.JumpCircle, _cameraConfigureAsset.RotationCameraGroupList);
                    AddCameraGroupTag(CameraTag.JumpFall, _cameraConfigureAsset.JumpNormalFallCameraGroupList);
                    AddCameraGroupTag(CameraTag.JumpBladeAir, _cameraConfigureAsset.JumpBladeAirCameraGroupList);
                    AddCameraGroupTag(CameraTag.JumpBladePre, _cameraConfigureAsset.JumpBladePreCameraGroupList);
                }
                else
                {
                    CameraDebugger.LogError("Load CameraConfigure Is Null");
                }
            }
        }

        /// <summary>
        /// 缓存相机数据
        /// </summary>
        /// <param name="cameraTag"></param>
        /// <param name="cameraGroupConfigureList"></param>
        private void AddCameraGroupTag(CameraTag cameraTag,List<CameraGroupConfigure> cameraGroupConfigureList)
        {
            _cameraGroupConfigureDic.Add(cameraTag,cameraGroupConfigureList);
        }

        #region API

        public CameraGroupConfigure GetCameraGroupConfigure(CameraTag tag)
        {
            if (_cameraGroupConfigureDic.ContainsKey(tag))
            {
                var list = _cameraGroupConfigureDic[tag];
                if (list != null && list.Count != 0)
                {
                    var rangeIndex = Random.Range(0, list.Count);
                    var item = list[rangeIndex];
                    return item;
                }
            }

            CameraDebugger.LogError("CameraGroupConfigure Has Not This CameraTag : "+tag);
            return null;
        }

        public CameraConfigureAsset GetCameraConfigure()
        {
            if (_cameraConfigureAsset == null)
            {
                CameraDebugger.LogError("CameraConfigureAsset Is Null");
                InitCameraConfigure();
            }
            return _cameraConfigureAsset;
        }

        public CameraGroupInfo GetCameraGroupInfo(string key)
        {
            if (_cameraGroupInfoDic.ContainsKey(key))
            {
                return _cameraGroupInfoDic[key];
            }
            return null;
        }

        /// <summary>
        /// 根据Id获取资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CameraPathConfigure GetCameraPathConfigure(int id)
        {
            if (_cameraPathConfigureDic != null)
            {
                if (_cameraPathConfigureDic.ContainsKey(id))
                {
                    return _cameraPathConfigureDic[id];
                }
            }
            else
            {
                CameraDebugger.LogError("CameraConfigureAsset Is Null");
                InitCameraConfigure();
            }

            return null;
        }

        #endregion
        
        /// <summary>
        /// 准备加载资源
        /// </summary>
        public void OnPrepareCameraDataList(List<CameraData> cameraDataList,Action callBack)
        {
            CameraPrepareEntity cameraPrepareEntity = _cameraModuleEntity.CameraEntityComponent.GetCameraPrepareInfo();
            cameraPrepareEntity.OnInitComponent(cameraDataList,callBack,_cameraModuleEntity);
            cameraPrepareEntity.OnPrepareAsset();
            _cameraPrepareInfoList.Add(cameraPrepareEntity);
        }

        public void OnPrepareSingleCameraData(CameraData cameraData,Action callBack)
        {
            List<CameraData> cameraDataList = new List<CameraData>();
            cameraDataList.Add(cameraData);
            OnPrepareCameraDataList(cameraDataList, callBack);
        }

        // protected override void OnUpdate(int updateTick)
        // {
        //     base.OnUpdate(updateTick);
        //     CheckPrepareState();
        // }

        /// <summary>
        /// 检查预加载状态
        /// </summary>
        private void CheckPrepareState()
        {
            if (_cameraPrepareInfoList!=null)
            {
                for (int i = 0; i < _cameraPrepareInfoList.Count; i++)
                {
                    CameraPrepareEntity cameraPrepareEntity = _cameraPrepareInfoList[i];
                    if (cameraPrepareEntity.CheckOnPrepared())
                    {
                        cameraPrepareEntity.OnPreparedCallBack();
                        cameraPrepareEntity.OnRelease();
                        _cameraPrepareInfoList.Remove(cameraPrepareEntity);
                    }
                }
            }
        }

        /// <summary>
        /// 添加虚拟相机
        /// </summary>
        /// <param name="cameraName"></param>
        /// <param name="tra"></param>
        public void AddVirtualCamera(string cameraName,Transform tra)
        {
            if (tra == null)
            {
                return;
            }
            
            //回收需要把物体放入指定根节点下
            tra.SetParent(_cameraModuleEntity.CameraRootNode.PoolRootTransform);
            tra.ResetTransform();
            if (VirtualCameraObjDic.ContainsKey(cameraName))
            {
                Queue<Transform> queue = VirtualCameraObjDic[cameraName];
                queue.Enqueue(tra);
            }
            else
            {
                Queue<Transform> queue = new Queue<Transform>(5);
                queue.Enqueue(tra);
                VirtualCameraObjDic.Add(cameraName,queue);
            }
        }

        /// <summary>
        /// 根据名字获取VirtualCamera
        /// </summary>
        /// <param name="cameraName"></param>
        /// <returns></returns>
        public Transform GetVirtualCamera(string cameraName)
        {
            if (VirtualCameraObjDic.ContainsKey(cameraName))
            {
                Queue<Transform> queue = VirtualCameraObjDic[cameraName];
                if (queue.Count > 0)
                {
                    return queue.Dequeue();
                }
                return null;
            }

            return null;
        }

        /// <summary>
        /// 根据key返回已经缓存的数据，数据不会更改。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public CameraAsset GetCameraAsset(string key)
        {
            if (CameraAssetDic.ContainsKey(key))
            {
                return CameraAssetDic[key];
            }

            return null;
        }
        
        /// <summary>
        /// 根据key返回已经缓存的数据，数据不会更改。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool HasCameraAsset(string key)
        {
            return CameraAssetDic.ContainsKey(key);
        }

        /// <summary>
        /// 设置存储
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cameraAsset"></param>
        public void SetCameraAsset(string key,CameraAsset cameraAsset)
        {
            if (!CameraAssetDic.ContainsKey(key))
            {
                CameraAssetDic.Add(key,cameraAsset);
            }
        }


        /// <summary>
        /// 战斗内获取相机数据，根据ChoreographyModule
        /// </summary>
        /// <returns></returns>
        public List<CameraData> GetCameraDataListByChoreographyModule()
        {
            List<CameraData> cameraDataList = new List<CameraData>();
            
            return cameraDataList;
        }
        
        public void OnClear()
        {
            _cameraModuleEntity.CameraAssetComponent.UnLoadMapAsset();
            CameraAssetDic.Clear();
            foreach (var transformQueue in VirtualCameraObjDic.Values)
            {
                for (int i = 0; i < transformQueue.Count; i++)
                {
                    Transform tra = transformQueue.Dequeue();
                    Object.DestroyImmediate(tra.gameObject);//可立即清理
                }
            }
            VirtualCameraObjDic.Clear();
        }
        
        #endregion
    }
}