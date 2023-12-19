using Module.Battle.Camera;
using UnityEngine;

namespace CameraModule.Runtime
{
    public class PathMarkEntity : MarkEntityBase
    {
        private bool _timeOffsetControl;
        private float _timeOffset;
        
        private bool _offsetControl;
        private CameraPathMarkOffsetType _offsetType;
        private Vector3 _offset;
        private Transform _targetRotationTransform;
        
        private Vector3 _markPos = Vector3.zero;
        private Quaternion _markRotation = Quaternion.identity;

        public override void Clear()
        {
            base.Clear();
            _timeOffsetControl = false;
            _timeOffset = 0;
            _offsetControl = false;
            _offsetType = CameraPathMarkOffsetType.Local;
            _offset = Vector3.zero;
            _targetRotationTransform = null;
            _markPos = Vector3.zero;
            _markRotation = Quaternion.identity;
        }

        public override CameraMarkType GetCameraMarkType()
        {
            return CameraMarkType.Path;
        }
        
        public override void OnUpdate(int updateTick,float frameTime)
        {
            base.OnUpdate(updateTick,frameTime);
            
            Vector3 position = Vector3.zero;
            Vector3 direction = Vector3.forward;
            Vector3 normal = Vector3.up;
            if (MarkTransform == null)
            {
                CameraDebugger.LogError("Camera Transform Is Null In PathMarkEntity");
                return;
            }
            
            _markRotation = Quaternion.LookRotation(direction, normal);
            if (_offsetControl)
            {
                if (_offsetType == CameraPathMarkOffsetType.World) _markPos = position + _offset;
                else
                {
                    var trs = Matrix4x4.TRS(position, _markRotation, Vector3.one);
                    _markPos = trs.MultiplyPoint3x4(_offset);
                }
            }
            else
            {
                _markPos = PosTargetTransform.position;
            }
            
            if (RotationTargetTransform!=null)
            {
                // _markRotation = RotationTargetTransform.rotation;
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

        public override void OnRelease()
        {
            base.OnRelease();
        }
    }
}