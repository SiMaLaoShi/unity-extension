using UnityEngine;

namespace Lib.Runtime.Extensions
{
    public static partial class Extension 
    {
        public static void SetLayer(this GameObject gameObj, string layerName)
        {
            var layer = LayerMask.NameToLayer(layerName);
            var trans = gameObj.GetComponentsInChildren<Transform>(true);
            foreach(Transform t in trans)
            {
                t.gameObject.layer = layer;
            }
        }
        
        /// <summary>
        /// 获取或者添加一个组件
        /// </summary>
        /// <param name="go"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            if (go != null)
            {
                var t = go.GetComponent<T>();
                if (t == null)
                    return go.AddComponent<T>();
            }

            return null;
        }
        
        /// <summary>
        /// 添加组件
        /// </summary>
        public static T Add<T>(this GameObject go) where T : Component
        {
            if (go != null)
            {
                T[] ts = go.GetComponents<T>();
                for (int i = 0; i < ts.Length; i++)
                {
                    if (ts[i] != null)
                        GameObject.Destroy(ts[i]);
                }
                return go.gameObject.AddComponent<T>();
            }
            return null;
        }
        
        /// <summary>
        /// 搜索子物体组件-GameObject版
        /// </summary>
        public static T GetChild<T>(this GameObject go, string subnode) where T : Component
        {
            if (go != null)
            {
                Transform sub = go.transform.Find(subnode);
                if (sub != null)
                    return sub.GetComponent<T>();
            }
            return null;
        }
    }
}