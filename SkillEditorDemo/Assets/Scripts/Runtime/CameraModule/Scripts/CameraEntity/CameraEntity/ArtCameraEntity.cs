using Animancer;
using UnityEngine;
using CameraType = Module.Battle.Camera.CameraType;

namespace CameraModule.Runtime
{
    public class ArtCameraEntity : CameraEntity
    {
        protected AnimancerComponent AnimancerComponent;
        private AnimancerState _state;
        private float _durationOffset;
        private AnimationClip[] _animations;
        private int _curAnimationIndex;
        private Transform _parentTransform;

        public override void InstanceObj()
        {
            base.InstanceObj();
            _parentTransform = CameraModuleEntity.CameraPoolComponent.GetArtEntityParentEmptyGameObject(CameraTransform.name);
            if (_parentTransform!=null)
            {
                CameraTransform.SetParent(_parentTransform);
                _parentTransform.position = CameraData.Pos;
                _parentTransform.rotation = CameraData.Rotation;
                _parentTransform.SetSiblingIndex(CameraData.Index);
            }
        }

        protected override void InitCinemachineComponent()
        {
            base.InitCinemachineComponent();
            if (CameraTransform!=null)
            {
                AnimancerComponent = CameraTransform.GetComponent<AnimancerComponent>();
            }
            else
            {
                CameraDebugger.LogError("Art CameraEntity Transform Is Null");
            }
            
            _curAnimationIndex = 0;
            _durationOffset = 0;
            if (CameraData.CameraAsset != null && CameraData.CameraAsset.AnimationClip != null)
            {
                _animations = CameraData.CameraAsset.AnimationClip;
                if (_animations.Length != 0&&_animations[0]!=null)
                {
                    _state = AnimancerComponent.Play(_animations[0], 0, FadeMode.FromStart);
                    _state.Speed = 0;
                }
            }
            
        }

        public override void HandleEntityInfo()
        {
            base.HandleEntityInfo();
        }

        public override void OnRelease()
        {
            CameraModuleEntity.CameraPoolComponent?.OnReleaseEmptyGameObject(_parentTransform);
            base.OnRelease();
        }

        
        public override void Clear()
        {
            base.Clear();
            AnimancerComponent = null;
            _state = null;
            _durationOffset = 0;
            _animations = null;
            _curAnimationIndex = 0;
            _parentTransform = null;
        }


        public override void OnUpdate(float time,float frameTime)
        {
            base.OnUpdate(time,frameTime);
            
            if (_state != null)
            {
                if (time >= _durationOffset + _state.Clip.length&&_animations.Length>_curAnimationIndex+1)
                {
                    if (AnimancerComponent == null)
                    {
                        CameraDebugger.LogError("artCamera has no AnimancerComponent");
                        return;
                    }
                    _durationOffset += _state.Clip.length;
                    _curAnimationIndex++;
                    _state = AnimancerComponent.Play(_animations[_curAnimationIndex], 0, FadeMode.FromStart);
                    _state.Speed = 0;
                }
            }
            if (_state != null)
            {
                _state.Time = time - _durationOffset;
            }
        }

        public override CameraEntityType GetCameraType()
        {
            return CameraEntityType.Art;
        }
        
    }
}