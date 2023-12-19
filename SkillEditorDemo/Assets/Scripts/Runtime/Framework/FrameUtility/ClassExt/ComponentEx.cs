using UnityEngine;

namespace Module.Utility
{
    public static class ComponentEx
    {
        /// <summary>
        ///     获取或者创建组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <returns></returns>
        public static T GetOrCreateComponent<T>(this GameObject go) where T : Component
        {
            var comp = go.GetComponent<T>();
            if (comp == null) comp = go.AddComponent<T>();
            return comp;
        }

        /// <summary>
        ///     在父节点中找到Component
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <returns></returns>
        public static T GetComponentInParent<T>(this GameObject go) where T : Component
        {
            if (go.transform.parent == null)
                return null;

            var parent = go.transform.parent.gameObject;
            var res = parent.GetComponent<T>();
            if (res == null)
                return GetComponentInParent<T>(parent);
            return res;
        }

        public static T CopyComponent<T>(this GameObject org, GameObject des) where T : Component
        {
            var orgComp = org.GetComponent<T>();
            if (!orgComp) return null;
            var desComp = des.GetComponent<T>();
            if (!desComp) desComp = des.AddComponent<T>();
            var type = orgComp.GetType();
            var fields = type.GetFields();
            foreach (var field in fields) field.SetValue(desComp, field.GetValue(orgComp));
            return desComp;
        }
    }
}