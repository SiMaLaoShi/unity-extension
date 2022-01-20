using System.Collections.Generic;
using UnityEngine;

namespace Lib.Runtime.Extensions
{
    public static partial class Extension
    {
        /// <summary>
        /// 搜索子物体组件-Transform版
        /// </summary>
        public static T GetChild<T>(this Transform trans, string subnode) where T : Component
        {
            if (trans != null)
            {
                Transform sub = trans.Find(subnode);
                if (sub != null)
                    return sub.GetComponent<T>();
            }
            return null;
        }

        /// <summary>
        /// 获取或者添加一个组件
        /// </summary>
        /// <param name="trans"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetOrAddComponent<T>(this Transform trans) where T : Component
        {
            if (trans != null)
            {
                var t = trans.GetComponent<T>();
                if (t == null)
                    return trans.gameObject.AddComponent<T>();
            }

            return null;
        }
        
        /// <summary>
        /// 查找子对象
        /// </summary>
        public static GameObject GetChild(this Transform trans, string subnode)
        {
            Transform tran = trans.Find(subnode);
            if (tran == null)
                return null;
            return tran.gameObject;
        }
        
        /// <summary>
        /// 取平级对象
        /// </summary>
        public static GameObject GetPeer(this Transform trans, string subnode)
        {
            Transform tran = trans.parent.Find(subnode);
            if (tran == null)
                return null;
            return tran.gameObject;
        }
        
        /// <summary>
        /// 清除所有子节点
        /// </summary>
        public static void ClearChild(this Transform trans)
        {
            if (trans == null)
                return;
            for (int i = trans.childCount - 1; i >= 0; i--)
            {
                GameObject.Destroy(trans.GetChild(i).gameObject);
            }
        }
        
        /// <summary>
        /// 设置层
        /// </summary>
        /// <param name="root"></param>
        /// <param name="layerName"></param>
        public static void SetLayer(this Transform root, string layerName)
        {
            int layer = LayerMask.NameToLayer(layerName);
            if (root != null)
            {
                Stack<Transform> children = new Stack<Transform>();
                children.Push(root);
                while (children.Count > 0)
                {
                    Transform currentTransform = children.Pop();
                    currentTransform.gameObject.layer = layer;
                    foreach (Transform child in currentTransform)
                    {
                        children.Push(child);
                    }
                }
            }
        }
    }
}