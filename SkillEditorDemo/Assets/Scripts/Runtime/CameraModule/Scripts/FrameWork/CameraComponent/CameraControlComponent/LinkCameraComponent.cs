using System;
using System.Collections.Generic;
using Cinemachine;
using Module.GameCore;

namespace CameraModule.Runtime
{
    public class LinkCameraComponent : ControlBaseComponent
    {
        private List<CameraData> _currentValidCameraDataList;
        private List<CameraData> _prepareCameraDataList;
        private List<DampingRecorderData> _recorderDampingDataList;
        private List<SingleCameraComponent> _preActiveCameraList;
        private Dictionary<CameraData, SingleCameraComponent> _currentPlaySingleCameraControlDic;
        private Action _initCallBack;
        private int mBrainOverrideId = -1;

        public int MBrainOverrideId => mBrainOverrideId;

        public override void OnInit(CinemachineBrain cinemachineBrain,CameraModuleEntity cameraModuleEntity)
        {
            base.OnInit(cinemachineBrain,cameraModuleEntity);

            if (_recorderDampingDataList == null)
            {
                _recorderDampingDataList = new List<DampingRecorderData>(5);
            }

            if (_preActiveCameraList == null)
            {
                _preActiveCameraList = new List<SingleCameraComponent>();
            }

            if (_currentPlaySingleCameraControlDic== null)
            {
                _currentPlaySingleCameraControlDic = new Dictionary<CameraData, SingleCameraComponent>(5);
            }

            if (_currentValidCameraDataList == null)
            {
                _currentValidCameraDataList = new List<CameraData>();
            }
        }

        public override void OnTick(int updateTicks)
        {
            base.OnTick(updateTicks);
        }

        /// <summary>
        /// 检测是否准备完毕
        /// </summary>
        protected override void CheckInitializationState()
        {
            base.CheckInitializationState();
            if (_initCallBack != null)
            {
                bool isInited = true;
                foreach (var cameraData in _prepareCameraDataList)
                {
                    if (cameraData.CameraAsset != null)
                    {
                        isInited = false;
                        break;
                    }
                }

                //准备完毕
                if (isInited)
                {
                    CameraWorkState = CameraWorkState.InitializationCompleted;
                    _initCallBack.Invoke();
                    _initCallBack = null;
                }
            }
        }
        
        public override void OnInitialization()
        {
            base.OnInitialization();
            CameraWorkState = CameraWorkState.Initialization;
            if (_prepareCameraDataList != null && _prepareCameraDataList.Count > 0)
            {
                for (int i = 0; i < _prepareCameraDataList.Count; i++)
                {
                    CameraData cameraData = _prepareCameraDataList[i];
                    cameraData.Index = i;
                    if (cameraData.CameraAsset == null)
                    {
                        CameraModuleEntity.CameraPoolComponent.PrepareCameraData(cameraData);
                    }
                    if (cameraData.CameraAsset.CurCameraType == Module.Battle.Camera.CameraType.AnimationCamera && cameraData.CameraAsset.TransitionTime > 0&&i > 0)
                    {
                        _prepareCameraDataList[i - 1].DurationTime += cameraData.CameraAsset.TransitionTime;
                        _prepareCameraDataList[i - 1].RefreshData();
                    }
                    cameraData.RefreshData();
                }
            }
            else
            {
                CameraDebugger.Log("Link Camera Asset Count Is Null");
            }
                
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="cameraDataList"></param>
        /// <param name="prepareCallBack"></param>
        public void SetData(List<CameraData> cameraDataList,Action prepareCallBack = null)
        {
            _initCallBack = prepareCallBack;
            _prepareCameraDataList = cameraDataList;
        }

        public override void OnStop()
        {
            base.OnStop();
            if (_currentPlaySingleCameraControlDic.Count > 0)
            {
                List<SingleCameraComponent> clearSingleCameraControlList = new List<SingleCameraComponent>();
                foreach (var singleCameraControl in _currentPlaySingleCameraControlDic.Values)
                {
                    clearSingleCameraControlList.Add(singleCameraControl);
                }

                for (int i = 0; i < clearSingleCameraControlList.Count; i++)
                {
                    SingleCameraComponent singleCameraControl = clearSingleCameraControlList[i];
                    var cinematic = singleCameraControl.GetCinemachineVirtualCamera();
                    cinematic.gameObject.SetActive(false);
                    singleCameraControl.OnStop();
                    singleCameraControl.OnRelease();
                }
                clearSingleCameraControlList.Clear();
                _currentPlaySingleCameraControlDic.Clear();
            }
        }

        public void DriverLinkCameraDataProcess(float time,float frameTime)
        {
            if (CinemachineBrain != null)
            {
                ResumeDamping();
                int activeInputs = 0;
                int clipIndexA = -1;
                int clipIndexB = -1;
                bool incomingIsA = false; 
                double weightB = 1;
                for (int i = 0; i < _prepareCameraDataList.Count; i++)
                {
                    double weight = GetCameraDataWeight(time,i);
                    CameraData cameraData = _prepareCameraDataList[i];
                    if (weight > 0)
                    {
                        AddSingleCameraControl(cameraData,time,frameTime);
                        
                        clipIndexA = clipIndexB;
                        clipIndexB = i;
                        weightB = weight;
                        if (++activeInputs == 2)
                        {
                            // Deduce which clip is incoming (timeline doesn't know)
                            var cameraDataA = _prepareCameraDataList[clipIndexA];
                            // Incoming has later start time (therefore earlier current time)
                            incomingIsA = cameraData.StartTime >= cameraDataA.StartTime; 
                            // If same start time, longer clip is incoming
                            if (Math.Abs(cameraData.StartTime - cameraDataA.StartTime) < 0.00001)
                            {
                                incomingIsA = cameraData.DurationTime < cameraDataA.DurationTime;
                            }
                            break;
                        }
                    }
                    else
                    {
                         // 非预览模式需要释放资源
                         if (!CameraModuleEntity.IsEditorModel)
                         {
                             if (OnReleaseSingleCameraControl(cameraData))
                             {
                                 i--;
                             }
                         }
                         else
                         {
                             //预览模式需要对相机层级做处理
                             OnReleaseEditorSingleCameraControl(cameraData);
                         }
                    }
                }
                
                if (activeInputs == 1 && weightB < 1 && _prepareCameraDataList[clipIndexB].StartTime > _prepareCameraDataList[clipIndexB].DurationTime / 2)
                {
                    incomingIsA = true;
                }
                if (incomingIsA)
                {
                    (clipIndexA, clipIndexB) = (clipIndexB, clipIndexA);
                    weightB = 1 - weightB;
                }

                ICinemachineCamera camA = null;
                if (clipIndexA >= 0)
                {
                    CameraData data = _prepareCameraDataList[clipIndexA];
                    if (_currentPlaySingleCameraControlDic[data] != null)
                    {
                        camA = _currentPlaySingleCameraControlDic[data].GetCinemachineVirtualCamera();
                        if (!_preActiveCameraList.Contains(_currentPlaySingleCameraControlDic[data]))
                        {
                            if (!CameraModuleEntity.IsEditorModel)
                            {
                                RecordDamping(_currentPlaySingleCameraControlDic[data]);
                            }
                        }
                    }
                    else
                    {
                        CameraDebugger.LogError("SingleCameraControl Data Is Null In CurrentPlayCameraDic");
                    }
                }

                ICinemachineCamera camB = null;
                if (clipIndexB >= 0)
                {
                    
                    CameraData data = _prepareCameraDataList[clipIndexB];
                    if (_currentPlaySingleCameraControlDic[data] != null)
                    {
                        camB = _currentPlaySingleCameraControlDic[data].GetCinemachineVirtualCamera();
                        
                        if (!_preActiveCameraList.Contains(_currentPlaySingleCameraControlDic[data]))
                        {
                            if (!CameraModuleEntity.IsEditorModel)
                            {
                                RecordDamping(_currentPlaySingleCameraControlDic[data]);
                            }
                        }
                    }
                    else
                    {
                        CameraDebugger.LogError("SingleCameraControl Data Is Null In CurrentPlayCameraDic");
                    }
                }
                _preActiveCameraList.Clear();
                
                foreach (var singleCameraControl in _currentPlaySingleCameraControlDic.Values)
                {
                    _preActiveCameraList.Add(singleCameraControl);
                    singleCameraControl.DriverSingleCameraProcess(time-singleCameraControl.CameraData.StartTime,frameTime);
                }
                
                mBrainOverrideId = CinemachineBrain.SetCameraOverride(mBrainOverrideId, camA, camB, (float)weightB, frameTime);
            }
            
        }

        /// <summary>
        /// 添加需要播放的单个相机
        /// </summary>
        /// <param name="cameraData"></param>
        /// <param name="time"></param>
        /// <param name="frameTime"></param>
        private void AddSingleCameraControl(CameraData cameraData,float time,float frameTime)
        {
            if (!_currentPlaySingleCameraControlDic.ContainsKey(cameraData))
            {
                var singleCameraComponent = CameraModuleEntity.CameraPoolComponent.GetSingleCameraComponent();
                singleCameraComponent.OnInit(CinemachineBrain,CameraModuleEntity);
                singleCameraComponent.SetData(cameraData);
                singleCameraComponent.OnInitialization();
                singleCameraComponent.ChangeCameraWorkState(CameraWorkState.Working);
                singleCameraComponent.DriverSingleCameraProcess(time-singleCameraComponent.CameraData.StartTime,frameTime);
                _currentPlaySingleCameraControlDic.Add(cameraData,singleCameraComponent);
            }
            else
            {
                var singleCameraControl = _currentPlaySingleCameraControlDic[cameraData];
                singleCameraControl.ChangeCameraWorkState(CameraWorkState.Working);
                singleCameraControl.DriverSingleCameraProcess(time-singleCameraControl.CameraData.StartTime,frameTime);
            }
        }

        /// <summary>
        /// 编辑器模式下预览，只需要将Camera的层级降低即可
        /// </summary>
        /// <param name="cameraData"></param>
        private void OnReleaseEditorSingleCameraControl(CameraData cameraData)
        {
            if (_currentPlaySingleCameraControlDic.ContainsKey(cameraData))
            {
                SingleCameraComponent singleCameraControl = _currentPlaySingleCameraControlDic[cameraData];
                if (singleCameraControl.CameraWorkState == CameraWorkState.Working)
                {
                    singleCameraControl.ChangeCameraWorkState(CameraWorkState.Free);
                }
            }
        }

        /// <summary>
        /// 释放接口
        /// </summary>
        /// <param name="cameraData"></param>
        private bool OnReleaseSingleCameraControl(CameraData cameraData)
        {
            if (_currentPlaySingleCameraControlDic.ContainsKey(cameraData))
            {
                SingleCameraComponent singleCameraControl = _currentPlaySingleCameraControlDic[cameraData];
                if (singleCameraControl.CameraWorkState == CameraWorkState.Working)
                {
                    _currentPlaySingleCameraControlDic.Remove(cameraData);
                    _prepareCameraDataList.Remove(cameraData);
                    if (_preActiveCameraList.Contains(singleCameraControl))
                    {
                        _preActiveCameraList.Remove(singleCameraControl);
                    }
                    singleCameraControl.OnRelease();
                    return true;
                }
            }

            return false;
        }
        
        private void RecordDamping(SingleCameraComponent singleCameraControl)
        {
            if (singleCameraControl ==null ||singleCameraControl.GetCameraEntityType() == CameraEntityType.Art || singleCameraControl.GetCinemachineVirtualCamera() == null) return;
            ICinemachineCamera camera = singleCameraControl.GetCinemachineVirtualCamera();
            if (camera is CinemachineVirtualCamera vcam)
            {
                DampingRecorderData recorderData = CameraModuleEntity.CameraPoolComponent.GetDampingRecorderData();
                recorderData.SingleCameraComponent = singleCameraControl;
                _recorderDampingDataList.Add(recorderData);
                CinemachineTransposer body = vcam.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachineTransposer;
                if (body != null)
                {
                    recorderData.XDamping = body.m_XDamping;
                    recorderData.YDamping = body.m_YDamping;
                    recorderData.ZDamping = body.m_ZDamping;
                    recorderData.YawDamping = body.m_YawDamping;
                    body.m_XDamping = 0;
                    body.m_YDamping = 0;
                    body.m_ZDamping = 0;
                    body.m_YawDamping = 0;
                }
                CinemachineComposer aim = vcam.GetCinemachineComponent(CinemachineCore.Stage.Aim) as CinemachineComposer;
                if (aim != null)
                {
                    recorderData.HorizontalDamping = aim.m_HorizontalDamping;
                    recorderData.VerticalDamping = aim.m_VerticalDamping;
                    aim.m_HorizontalDamping = 0;
                    aim.m_VerticalDamping = 0;
                }
            }
        }
        
        private void ResumeDamping()
        {
            if (_recorderDampingDataList == null || _recorderDampingDataList.Count == 0) return;
            for (int i = 0; i < _recorderDampingDataList.Count; i++)
            {
                var record = _recorderDampingDataList[i];
                if (record.SingleCameraComponent != null&&record.SingleCameraComponent.GetCameraEntityType() == CameraEntityType.Normal)
                {
                    if (record.SingleCameraComponent.GetCinemachineVirtualCamera()!=null&&record.SingleCameraComponent.GetCinemachineVirtualCamera() is CinemachineVirtualCamera vcam)
                    {
                        CinemachineTransposer body = vcam.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachineTransposer;
                        if (body != null)
                        {
                            body.m_XDamping = record.XDamping;
                            body.m_YDamping = record.YDamping;
                            body.m_ZDamping = record.ZDamping;
                            body.m_YawDamping = record.YawDamping;
                        }
                        CinemachineComposer aim = vcam.GetCinemachineComponent(CinemachineCore.Stage.Aim) as CinemachineComposer;
                        if (aim != null)
                        {
                            aim.m_HorizontalDamping = record.HorizontalDamping;
                            aim.m_VerticalDamping = record.VerticalDamping;
                        }
                    }
                }
                CameraModuleEntity.CameraPoolComponent.OnReleaseDampingRecorderData(record);
            }
            _recorderDampingDataList.Clear();
        }
        
        private double GetCameraDataWeight(double curTimer, int index)
        {
            // Initialize a list of valid nodes
            if (_currentValidCameraDataList == null)
                return 0;
            _currentValidCameraDataList.Clear();
            // Loop through the list of time nodes
            foreach (CameraData cameraData in _prepareCameraDataList)
            {
                // Check if the node is valid
                if (curTimer >=cameraData.StartTime && curTimer < cameraData.EndTime)
                {
                    // Add it to the list of valid nodes
                    _currentValidCameraDataList.Add(cameraData);
                }
            }

            // Check the number of valid nodes
            if (_currentValidCameraDataList.Count == 0)
            {
                // Return 0 as the weight for any index
                return 0;
            }
            else if (_currentValidCameraDataList.Count == 1)
            {
                // Return 1 as the weight for that index and 0 for any other index
                return index == _prepareCameraDataList.IndexOf(_currentValidCameraDataList[0]) ? 1 : 0;
            }
            else if (_currentValidCameraDataList.Count == 2)
            {
                // Calculate the overlap start and end time
                double blendStart = Math.Max(_currentValidCameraDataList[0].StartTime, _currentValidCameraDataList[1].StartTime);
                double blendEnd = Math.Min(_currentValidCameraDataList[0].EndTime, _currentValidCameraDataList[1].EndTime);
                if (Math.Abs(blendEnd - blendStart) <= float.Epsilon)
                {
                    return 0;
                }

                // Calculate the weight for each node
                double weight1 = (curTimer - blendStart) /  (blendEnd - blendStart);
                double weight2 = (blendEnd - curTimer) /  (blendEnd - blendStart);

                // Return the weight for the given index or 0 if out of range
                if (index == _prepareCameraDataList.IndexOf(_currentValidCameraDataList[0]))
                {
                    return weight2;
                }
                else if (index == _prepareCameraDataList.IndexOf(_currentValidCameraDataList[1]))
                {
                    return weight1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public override void Clear()
        {
            base.Clear();
            _preActiveCameraList.Clear();
            _currentValidCameraDataList.Clear();
            _currentPlaySingleCameraControlDic.Clear();
            _recorderDampingDataList.Clear();
            _prepareCameraDataList.Clear();
            _initCallBack = null;
            mBrainOverrideId = -1;
        }

        public override void OnRelease()
        {
            base.OnRelease();
        }
    }
}