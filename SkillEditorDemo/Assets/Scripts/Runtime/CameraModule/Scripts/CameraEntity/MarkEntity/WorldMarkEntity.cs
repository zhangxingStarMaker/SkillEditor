using Module.Battle.Camera;
using UnityEngine;

namespace CameraModule.Runtime
{
    public class WorldMarkEntity : MarkEntityBase
    {
        private Vector3 _pointerPosition;
        private Quaternion _pointerRotation;

        public override CameraMarkType GetCameraMarkType()
        {
            return CameraMarkType.World;
        }

        public override void OnUpdate(int updateTick,float frameTime)
        {
            base.OnUpdate(updateTick,frameTime);
            
            if (MarkTransform == null)
            {
                CameraDebugger.LogError("Camera Transform Is Null In WordMarkEntity");
                return;
            }
            
            if (!IsSlow)
            {
                MarkTransform.position = _pointerPosition;
                MarkTransform.rotation = _pointerRotation;
            }
            else
            {
                MarkTransform.position = Vector3.Slerp(MarkTransform.position, _pointerPosition, Speed * frameTime);

                MarkTransform.rotation = Quaternion.Slerp(MarkTransform.rotation, _pointerRotation, Speed * frameTime);
            }
        }

        public override void OnRelease()
        {
            base.OnRelease();
        }

        public override void Clear()
        {
            base.Clear();
            _pointerPosition = Vector3.zero;
            _pointerRotation = Quaternion.identity;
        }
    }
}