using UnityEditor;
using UnityEngine;

namespace Lib.Editor.Inspector
{
    // [CanEditMultipleObjects]
    // [CustomEditor(typeof(RectTransform), true)]
    public class RectTransformInspector : BaseInspector
    {
        public RectTransformInspector(string editorTypeName) : base(editorTypeName)
        {
        }
        
        internal RectTransformInspector()
            : base("RectTransformInspector") { }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            // DrawCopyToSystemBuffer();
        }

        void DrawCopyToSystemBuffer()
        {
            var sContents = Contents.instance;
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button(sContents.contentCopyPos, sContents.styleButtonLeft))
                {
                    var rectTrans = target as RectTransform;
                    GUIUtility.systemCopyBuffer = rectTrans.anchoredPosition3D.ToString();
                }

                if (GUILayout.Button(sContents.contentCopyScale, sContents.styleButtonMid))
                {
                    var rectTrans = target as RectTransform;
                    GUIUtility.systemCopyBuffer = rectTrans.localScale.ToString();
                }

                if (GUILayout.Button(sContents.contentCopyPath, sContents.styleButtonMid))
                {
                    var rectTrans = target as RectTransform;
                    string node = string.Empty;
                    CommonUtility.GetNodePath(rectTrans, ref node);
                    GUIUtility.systemCopyBuffer = node;
                }

                if (GUILayout.Button(sContents.contentCopySize, sContents.styleButtonRight))
                {
                    var rectTrans = target as RectTransform;
                    GUIUtility.systemCopyBuffer = rectTrans.sizeDelta.ToString();
                }
            }
            GUILayout.EndHorizontal();
        }
        
          private class Contents : Singleton<Contents>
        {
            public readonly GUIContent contentColorLib = new GUIContent("ColorLib", "Set by basic_colors");
            public readonly GUIContent contentCopy = new GUIContent("Copy", "Copy component value");

            public readonly GUIContent contentCopyPos = new GUIContent("CPos", "Copy pos");
            public readonly GUIContent contentCopySize = new GUIContent("CSize", "Copy size");
            public readonly GUIContent contentCopyScale = new GUIContent("CScale", "Copy scale");
            public readonly GUIContent contentCopyPath = new GUIContent("CPath", "Copy node path");
            public readonly GUIContent contentFilled = new GUIContent("Filled", "Fill the parent RectTransform");
            public readonly GUIContent contentPaste = new GUIContent("Paste", "Paste component value");

            public readonly GUIContent contentReadme =
                new GUIContent("Help", "Ctrl+Arrow key move rectTransform\nCtrl: 1px    Shift: 10px");

            public readonly GUIContent contentSizeLib = new GUIContent("SizeLib", "Size Lib");
            public readonly GUIContent contentTextLib = new GUIContent("TextLib", "Text Lib");

            public readonly GUIContent contentUnfilled = new GUIContent("Unfilled", "Change to normal sizeDelta mode");

            public readonly GUIStyle styleButtonLeft = new GUIStyle("ButtonLeft");
            public readonly GUIStyle styleButtonMid = new GUIStyle("ButtonMid");
            public readonly GUIStyle styleButtonRight = new GUIStyle("ButtonRight");
            public readonly GUIStyle stylePivotSetup;

            public Contents()
            {
                stylePivotSetup = new GUIStyle("PreButton")
                {
                    normal = new GUIStyle("CN Box").normal,
                    active = new GUIStyle("AppToolbar").normal,
                    overflow = new RectOffset(),
                    padding = new RectOffset(1, -1, -1, 1),
                    fontSize = 15,
                    fixedWidth = 15,
                    fixedHeight = 15
                };
            }
        }
    }
}