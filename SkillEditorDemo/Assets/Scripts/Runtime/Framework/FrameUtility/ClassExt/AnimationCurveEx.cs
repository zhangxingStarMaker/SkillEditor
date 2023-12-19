using Runtime.Framework.FrameUtility;
using UnityEngine;

namespace Module.Utility
{
    public static class AnimationCurveEx
    {
        //根据当前时间总比例获取曲线对应的时间
        public static float GetCurveTimeByProgress(this AnimationCurve curve,float progress)
        {
            if (curve == null || curve.length < 1)
            {
                return 0;
            }
            return curve.GetCurveTotalTime() * progress;
        }

        public static void CopyTo(this AnimationCurve curve,AnimationCurve targetCurve)
        {
            if (curve==null || targetCurve == null)
            {
                FrameworkLog.LogError($"CopyTo error:curve==null || targetCurve == null!");
                return;
            }

            targetCurve.keys = curve.keys;
            targetCurve.preWrapMode = curve.preWrapMode;
            targetCurve.postWrapMode = curve.postWrapMode;
        }
        
        public static float GetCurveTotalTime(this AnimationCurve curve)
        {
            if (curve == null )
            {
                return 0;
            }
            return curve.keys[curve.length - 1].time;
        }
        
        public static int GetCurveTotalMsTime(this AnimationCurve curve)
        {
            if (curve == null )
            {
                return 0;
            }

            var totalTime = curve.GetCurveTotalTime();
            return UnityEngine.Mathf.CeilToInt(totalTime * 1000);
        }
    }
}