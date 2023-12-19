using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Module.Utility
{
    public static class TransformEx
    {
        /// <summary>
        ///     将一个物体旋转到指向另一个物体
        /// </summary>
        /// <param name="from">旋转的物体</param>
        /// <param name="to">被指向的物体</param>
        /// <returns>旋转物体的最终欧拉角</returns>
        public static Quaternion FaceTo(this Transform from, Transform to)
        {
            var euler = Vector2.Angle(to.transform.position - from.transform.position, Vector2.up);
            if (to.position.x - from.position.x > 0)
                return Quaternion.Euler(new Vector3(0, 0, -euler));
            return Quaternion.Euler(new Vector3(0, 0, euler));
        }

        /// <summary>
        ///     将一个物体旋转到指向一个点
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static Quaternion FaceTo(this Transform from, Vector3 to)
        {
            var euler = Vector2.Angle(to - from.transform.position, Vector2.up);
            if (to.x - from.position.x > 0)
                return Quaternion.Euler(new Vector3(0, 0, -euler));
            return Quaternion.Euler(new Vector3(0, 0, euler));
        }

        /// <summary>
        ///     将一个向量旋转到指向另一个向量所需的角度（有符号）
        /// </summary>
        /// <param name="from">旋转的向量</param>
        /// <param name="to">目标向量</param>
        /// <returns>旋转向量所需的角度（有符号）</returns>
        public static float EulerRotateTo2D(this Vector3 from, Vector3 to)
        {
            var euler = Vector2.Angle(from, to);
            if (to.x - from.x > 0)
                return -euler;
            return euler;
        }

        /// <summary>
        ///     极坐标转换
        /// </summary>
        /// <param name="tran"></param>
        /// <param name="point"></param>
        /// <param name="angle"></param>
        /// <param name="distance"></param>
        public static void PolarCoordinates(this Transform tran, Vector3 point, float angle, float distance)
        {
            var origDirection = new Vector3(0, distance, 0); //地基距离球心半径
            var rotation = Quaternion.Euler(0, 0, angle);
            tran.rotation = rotation; //垂直于地心
            tran.position = point + rotation * origDirection; //计算位置
        }
    }
}