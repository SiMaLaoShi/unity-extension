using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Reflection;
using System.Text;

public class UIRebuildLogger : MonoBehaviour {

    /// <summary>
    /// 是否开启帧模式，
    /// 帧模式下关心每一帧有哪些UI元素触发了Rebuild，
    /// 非帧模式下则关心UI元素触发了多少次Rebuild（建议开启Console的Collapse）
    /// </summary>
    [Tooltip("是否开启帧模式，\n帧模式下关心每一帧有哪些UI元素触发了Rebuild，\n非帧模式下则关心UI元素触发了多少次Rebuild（建议开启Console的Collapse）")]
    [SerializeField]
    bool m_FrameMode;

    IList<ICanvasElement> m_LayoutRebuildQueue;
    IList<ICanvasElement> m_GraphicRebuildQueue;
    StringBuilder sb = new StringBuilder();
    private void Awake()
    {
        DontDestroyOnLoad(this);
        Type type = typeof(CanvasUpdateRegistry);
        FieldInfo field = type.GetField("m_LayoutRebuildQueue", BindingFlags.NonPublic | BindingFlags.Instance);
        m_LayoutRebuildQueue = (IList<ICanvasElement>)field.GetValue(CanvasUpdateRegistry.instance);
        field = type.GetField("m_GraphicRebuildQueue", BindingFlags.NonPublic | BindingFlags.Instance);
        m_GraphicRebuildQueue = (IList<ICanvasElement>)field.GetValue(CanvasUpdateRegistry.instance);
    }

    private void Update()
    {
       

        for (int j = 0; j < m_LayoutRebuildQueue.Count; j++)
        {
            ICanvasElement element = m_LayoutRebuildQueue[j];
            if (!ObjectValidForUpdata(element))
            {
                continue;
            }

            Graphic graphic = element.transform.GetComponent<Graphic>();
            if (graphic == null)
            {
                continue;
            }

            Canvas canvas = graphic.canvas;
            if (canvas == null)
            {
                continue;
            }

            string str = "<color=#ff0000>" + element.transform.name + "</color>的LayoutRebuild引起<color=#ff0000>" + canvas.name + "</color>网格重建";

            if (m_FrameMode)
            {
                sb.AppendLine(str);
            }
            else
            {
                Debug.LogError(str);
            }
            
        }

        for (int j = 0; j < m_GraphicRebuildQueue.Count; j++)
        {
            ICanvasElement element = m_GraphicRebuildQueue[j];

            if (!ObjectValidForUpdata(element))
            {
                continue;
            }

            Graphic graphic = element.transform.GetComponent<Graphic>();
            if (graphic == null)
            {
                continue;
            }

            Canvas canvas = graphic.canvas;
            if (canvas == null)
            {
                continue;
            }


            string canvansName = canvas.name;
            if (canvansName == "DebugFps")
            {
                continue;
            }

            string str = "<color=#ff0000>" + element.transform.name + "</color>的LayoutRebuild引起<color=#ff0000>" + canvas.name + "</color>网格重建";

            if (m_FrameMode)
            {
                sb.AppendLine(str);
            }
            else
            {
                Debug.LogError(str);
            }
        }

        if (sb.Length != 0)
        {
            Debug.LogError("当前帧<color=#66ccff>" + Time.frameCount + "</color>:\n" + sb.ToString());
            sb = new StringBuilder();
        }
    }

    private bool ObjectValidForUpdata(ICanvasElement element)
    {
        bool valid = element != null;
        bool isUnityObject = element is UnityEngine.Object;

        if (isUnityObject)
        {
            valid  = (element as UnityEngine.Object) != null;
        }

        return valid;
    }
}
