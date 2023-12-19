using UnityEngine;

namespace Module.Utility
{
    public static class RectTransformEx
    {
        public static void Reset(this RectTransform rt,Vector3 anchoredPos,Vector3 rotation,Vector3 scale,Vector2 sizeDelta)
        {
            rt.anchoredPosition3D = anchoredPos;
            rt.localEulerAngles = rotation;
            rt.localScale = scale;
            rt.sizeDelta = sizeDelta;
        }
    }
}