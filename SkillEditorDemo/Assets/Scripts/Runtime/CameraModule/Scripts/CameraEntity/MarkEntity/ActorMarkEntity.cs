using Module.Battle.Camera;
using UnityEngine;

namespace CameraModule.Runtime
{
    public class ActorMarkEntity : MarkEntityBase
    {
        #region Property

        private bool _offsetControl;
        private CameraPlayerMarkOffsetType _offsetType;
        private Vector3 _offset;
        private CameraTarget _cameraTargetType;
        
        private Vector3 _markPos = Vector3.zero;
        private Quaternion _markRotation = Quaternion.identity;

        #endregion
        
        public override CameraMarkType GetCameraMarkType()
        {
            return CameraMarkType.Actor;
        }
        
        public override void OnUpdate(int updateTick,float frameTime)
        {
            base.OnUpdate(updateTick,frameTime);

            if (PosTargetTransform != null)
            {
                _markPos = Vector3.zero;
                _markRotation = Quaternion.identity;
                if (_offsetControl)
                {
                    if (_offsetType == CameraPlayerMarkOffsetType.World)
                    {
                        _markPos = PosTargetTransform.position + _offset;
                    }
                    else
                    {
                        var trs = Matrix4x4.TRS(PosTargetTransform.position, PosTargetTransform.rotation, Vector3.one);
                        if (_cameraTargetType == CameraTarget.Head)
                        {
                            Vector3 fDirection = PosTargetTransform.up;
                            Vector3 uDirection = -PosTargetTransform.right;
                            Quaternion rotation = Quaternion.LookRotation(fDirection, uDirection);
                            trs = Matrix4x4.TRS(PosTargetTransform.position, rotation, Vector3.one);
                        }
                        _markPos = trs.MultiplyPoint3x4(_offset);
                    }
                }
                else
                {
                    _markPos = PosTargetTransform.position;
                }

                if (RotationTargetTransform!=null)
                {
                    _markRotation = RotationTargetTransform.rotation;
                }

                if (!IsSlow)
                {
                    MarkTransform.position = _markPos;
                    MarkTransform.rotation = _markRotation;
                }
                else
                {
                    MarkTransform.position = Vector3.Slerp(MarkTransform.position, _markPos, Speed * frameTime);
                    MarkTransform.rotation = Quaternion.Slerp(MarkTransform.rotation, _markRotation, Speed * frameTime);
                }
                
            }
            else
            {
                CameraDebugger.LogError("TargetTransform Is Null");
            }
        }

        public override void OnRelease()
        {
            base.OnRelease();
            
        }

        public override void Clear()
        {
            base.Clear();
            _offsetControl = false;
            _offsetType = CameraPlayerMarkOffsetType.World;
            _offset = Vector3.zero;
            _cameraTargetType = CameraTarget.None;
        }
    }
}