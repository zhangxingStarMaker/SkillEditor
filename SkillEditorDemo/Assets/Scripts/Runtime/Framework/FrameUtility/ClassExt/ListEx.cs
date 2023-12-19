using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Mathf = UnityEngine.Mathf;

namespace Module.Utility
{
    public static class ListEx
    {
        /// <summary>
        ///     遍历列表
        /// </summary>
        /// <typeparam name="T">列表类型</typeparam>
        /// <param name="list">目标表</param>
        /// <param name="act">行为</param>
        public static void ForEach<T>(this List<T> list, Action<int, T> act)
        {
            for (var i = 0; i < list.Count; i++) act(i, list[i]);
        }

        /// <summary>
        ///     获得随机列表中元素
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="list">列表</param>
        /// <returns></returns>
        public static T GetRandomItem<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        /// <summary>
        ///     根据权值来获取索引
        /// </summary>
        /// <param name="powers"></param>
        /// <returns></returns>
        public static int GetRandomWithPower(this List<int> powers)
        {
            var sum = 0;
            for (var i = 0; i < powers.Count; i++) sum += powers[i];
            var randomNum = Random.Range(0, sum);
            var currentSum = 0;
            for (var i = 0; i < powers.Count; i++)
            {
                var nextSum = currentSum + powers[i];
                if (randomNum >= currentSum && randomNum <= nextSum) return i;
                currentSum = nextSum;
            }

            Debug.LogError("权值范围计算错误！");
            return -1;
        }

        public static void Append<T>(this List<T> target, List<T> from)
        {
            for (int i = 0; i < from.Count; i++)
            {
                target.Add(from[i]);
            }
        }

        /// <summary>
        ///     拷贝到
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        public static void CopyTo<T>(this List<T> from, List<T> to, int begin = 0, int end = -1)
        {
            if (begin < 0) begin = 0;
            var endIndex = UnityEngine.Mathf.Min(from.Count, to.Count) - 1;
            if (end != -1 && end < endIndex) endIndex = end;
            for (var i = begin; i < endIndex; i++) to[i] = from[i];
        }

        /// <summary>
        ///     将List转为Array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="from"></param>
        /// <returns></returns>
        public static T[] ToArraySavely<T>(this List<T> from)
        {
            var res = new T[from.Count];
            for (var i = 0; i < from.Count; i++) res[i] = from[i];
            return res;
        }

        /// <summary>
        ///     尝试获取，如果没有该数则返回null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="from"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T TryGet<T>(this List<T> from, int index)
        {
            if (from.Count > index) return from[index];
            return default(T);
        }

        public static string Join(this List<string> from,string concatStr)
        {
            var fromNum = from.Count;
            if (fromNum < 1)
            {
                return String.Empty;
            }

            string resultStr = "";
            // using (zstring.Block())
            // {
            //     zstring result = from[0];
            //     for (int i = 1; i < fromNum; i++)
            //     {
            //         result = result + concatStr + from[i];
            //     }
            //
            //     resultStr = result.Intern();
            // }
            return resultStr;
        }
    }
}