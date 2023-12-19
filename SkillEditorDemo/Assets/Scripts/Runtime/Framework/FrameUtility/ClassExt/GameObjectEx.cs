using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Module.Utility
{
    public static class GameObjectEx
    {
        /// <summary>
        /// 获取GameObj实例化后在场景中的路径
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public static string GetInstancePath(this GameObject go)
        {
            if (go == null)
            {
                return "";
            }
            var selectTran = go.transform;
            var parentTran = selectTran.parent;
            var path = selectTran.name;
            while (parentTran != null)
            {
                path = $"{parentTran.name}/{path}";
                parentTran = parentTran.parent;
            }
            return path;
        }
        
        /// <summary>
        ///     重置物体的Transform
        /// </summary>
        /// <param name="go"></param>
        public static void ResetTransform(this GameObject go)
        {
            go.transform.position = Vector3.zero;
            go.transform.eulerAngles = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.SetActiveEx(true);
        }
        
        public static void ResetLocalTransform(this GameObject go)
        {
            go.transform.localPosition = Vector3.zero;
            go.transform.localEulerAngles = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.SetActiveEx(true);
        }
        
        public static void SetActiveEx(this GameObject go, bool active)
        {
            if (go == null)
            {
                return;
            }
            if (go.activeSelf == active)
            {
                return;
            }

            go.SetActive(active);
        }

        /// <summary>
        ///     重置物体的Transfrom
        /// </summary>
        /// <param name="tran"></param>
        public static void ResetTransform(this Transform tran)
        {
            tran.position = Vector3.zero;
            tran.eulerAngles = Vector3.zero;
            tran.localScale = Vector3.one;
            tran.gameObject.SetActiveEx(true);
        }
        
        /// <summary>
        ///     重置物体的Transfrom
        /// </summary>
        /// <param name="tran"></param>
        public static void ResetRectTransform(this RectTransform tran)
        {
            tran.anchoredPosition3D = Vector3.zero;
            tran.eulerAngles = Vector3.zero;
            tran.localScale = Vector3.one;
            tran.gameObject.SetActiveEx(true);
        }
        public static Vector3 NewRotateAround(this Transform tran, Vector3 pos, Vector3 euler)
        {
            var rotation = Quaternion.Euler(euler) * tran.localRotation;
            var newPosition = rotation * (tran.position - pos);
            return newPosition;
        }
        
        /// <summary>
        ///     查找指定祖先节点下的组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <returns></returns>
        public static T GetComponentInChild<T>(this GameObject node) where T : Component
        {
            var ancestorNode = node.transform;
            for (var i = 0; i < ancestorNode.childCount; i++)
            {
                T tmp;
                tmp = ancestorNode.GetChild(i).gameObject.GetComponent<T>();
                if (tmp != null)
                    return tmp;
                tmp = GetComponentInChild<T>(ancestorNode.GetChild(i).gameObject);
                if (tmp != null)
                    return tmp;
            }

            return null;
        }

        /// <summary>
        ///     查找指定节点的 任意子节点
        /// </summary>
        /// <typeparam name="T">脚本类型</typeparam>
        /// <param name="ancestorNode"></param>
        /// <param name="name">对象名称路径</param>
        /// <returns>脚本</returns>
        public static T FindComponentInChild<T>(this Transform ancestorNode, string name) where T : Component
        {
            var t = ancestorNode.transform.Find(name);
            if (t) return t.gameObject.GetComponent<T>();
            return null;
        }

        private static void FindComponentsInChild<T>(GameObject nowNode, List<T> ls) where T : Component
        {
            var comp = nowNode.GetComponent<T>();
            if (comp) ls.Add(comp);
            var childNum = nowNode.transform.childCount;
            for (var i = 0; i < childNum; i++) FindComponentsInChild(nowNode.transform.GetChild(i).gameObject, ls);
        }

        /// <summary>
        ///     查找指定节点的 挂有T脚本的子节点
        /// </summary>
        /// <typeparam name="T">脚本类型</typeparam>
        /// <param name="node"></param>
        /// <param name="ls">结果队列</param>
        public static void FindComponentsInChildExt<T>(this GameObject node, List<T> ls) where T : Component
        {
            FindComponentsInChild(node, ls);
        }
    }
}