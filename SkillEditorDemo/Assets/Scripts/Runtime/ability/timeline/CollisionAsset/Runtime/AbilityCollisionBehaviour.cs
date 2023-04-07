using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;

namespace AbilitySystem
{
    [System.Serializable]
    public class AbilityCollisionBehaviour : PlayableBehaviour
    {
        // [LabelText("碰撞类型")]
        // public DetectionTypeEnum DetectionType = DetectionTypeEnum.None;

        [ShowIf("@this.DetectionType == DetectionTypeEnum.Box")] [LabelText("横截面大小")]
        public Vector2 BoxSize;

        [ShowIf("@this.DetectionType == DetectionTypeEnum.Circular || this.DetectionType == DetectionTypeEnum.Sector")]
        [LabelText("半径大小")]
        public float Radius;

        [ShowIf("@this.DetectionType == DetectionTypeEnum.Sector")] [LabelText("扇形百分比")]
        public float Percentage;

        [ShowIf("@this.DetectionType != DetectionTypeEnum.None")] [LabelText("高度")]
        public int Height;

        [ShowIf("@this.DetectionType != DetectionTypeEnum.None")] [LabelText("偏移位置")]
        public Vector3 OffsetPos;

        [ShowIf("@this.DetectionType != DetectionTypeEnum.None")] [LabelText("是否跟随")]
        public bool IsFollow;
    }
}