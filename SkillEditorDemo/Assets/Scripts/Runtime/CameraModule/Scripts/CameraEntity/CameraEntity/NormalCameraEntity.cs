using System.Collections.Generic;
using Module.Battle.Camera;
using CameraType = Module.Battle.Camera.CameraType;

namespace CameraModule.Runtime
{
    public class NormalCameraEntity : CameraEntity
    {
        private readonly List<MarkEntityBase> _followMarkEntityList = new List<MarkEntityBase>();
        private readonly List<MarkEntityBase> _lookAtMarkEntityList = new List<MarkEntityBase>();
      
        public override void InstanceObj()
        {
            base.InstanceObj();
            if (CameraTransform!=null)
            {
                CameraTransform.SetParent(CameraModuleEntity.CameraRootNode.NormalCameraRootTransform);
                CameraTransform.SetSiblingIndex(CameraData.Index);
            }
        }

        public override void HandleEntityInfo()
        {
            base.HandleEntityInfo();

            if (CameraData.CameraAsset != null)
            {
                CreateMarkEntity(CameraData.CameraAsset.CameraFollowMarks,_followMarkEntityList,CameraData.Index,"follow");
                CreateMarkEntity(CameraData.CameraAsset.CameraLookMarks,_lookAtMarkEntityList,CameraData.Index,"Look");
                BindCameraTarget();
            }
        }

        /// <summary>
        /// 绑定目标点
        /// </summary>
        private void BindCameraTarget()
        {
            if (_followMarkEntityList.Count == 1) CinemachineCamera.Follow = _followMarkEntityList[0].MarkTransform;
            
            // if (_followMarkEntityList.Count > 1&&FollowGroup!=null)
            // {
            //     foreach (var followMark in FollowMarkEntityList)
            //         FollowGroup.AddMember(followMark.CameraMarkObject.transform, followMark.Weight, 0);
            //     
            //     CinemachineCamera.Follow = FollowGroup.transform;
            // }

            if (_lookAtMarkEntityList.Count == 1) CinemachineCamera.LookAt = _lookAtMarkEntityList[0].MarkTransform;
            // if (LookMarkEntityList.Count > 1&&LookGroup!=null)
            // {
            //     foreach (var lookMark in LookMarkEntityList)
            //         LookGroup.AddMember(lookMark.CameraMarkObject.transform, lookMark.Weight, 0);
            //
            //     CinemachineCamera.LookAt = LookGroup.transform;
            // }
        }

        /// <summary>
        /// 创建Follow Entity
        /// </summary>
        /// <param name="cameraFollowMarks"></param>
        /// <param name="markEntityList"></param>
        /// <param name="index"></param>
        /// <param name="markName"></param>
        private void CreateMarkEntity(List<CameraMarkData> cameraFollowMarks,List<MarkEntityBase> markEntityList,int index,string markName)
        {
            int count = 0;
            foreach (var followMark in cameraFollowMarks)
            {
                count++;
                CameraMarkInfo cameraMarkInfo = CameraModuleEntity.CameraPoolComponent.GetCameraMarkInfo(followMark);
                if (cameraMarkInfo!=null)
                {
                    string name = $"{index}_{count}_" + markName;
                    cameraMarkInfo.Name = name;
                    // todo 后期优化，相机的目标不应该存放在CameraData里面，应该根据CameraDate里面的配置动态的去查找目标跟随点
                    MarkEntityBase markEntity = CameraModuleEntity.CameraEntityComponent.GetMarkEntity(cameraMarkInfo,CameraData.TargetTransform);
                    if (markEntity!=null)
                    {
                        markEntity.HandleTransform();
                        markEntityList.Add(markEntity);
                    }
                }
                else
                {
                    CameraDebugger.LogError("CameraMarkInfo Is Null");
                }
                
            }
        }

        public override void OnUpdate(float time,float frameTime)
        {
            base.OnUpdate(time,frameTime);
            for (int i = 0; i < _followMarkEntityList.Count; i++)
            {
                _followMarkEntityList[i].OnUpdate(1,frameTime);
            }
            for (int i = 0; i < _lookAtMarkEntityList.Count; i++)
            {
                _lookAtMarkEntityList[i].OnUpdate(1,frameTime);
            }
        }


        public override void Clear()
        {
            base.Clear();
            _followMarkEntityList.Clear();
            _lookAtMarkEntityList.Clear();
        }

        public override void OnRelease()
        {
            for (int i = 0; i < _followMarkEntityList.Count; i++)
            {
                _followMarkEntityList[i].OnRelease();
            }
            
            for (int i = 0; i < _lookAtMarkEntityList.Count; i++)
            {
                _lookAtMarkEntityList[i].OnRelease();
            }
            
            base.OnRelease();
        }

        public override CameraEntityType GetCameraType()
        {
            return CameraEntityType.Normal;
        }
    }
}