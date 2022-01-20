using System;
using UnityEngine;
using UnityEditor;
namespace Lib.Editor.Inspector
{
    // [CanEditMultipleObjects]
    // [CustomEditor(typeof(Transform), true)]
    public class TransformInspector : BaseInspector
    {
        public TransformInspector(string editorTypeName) : base(editorTypeName)
        {
            
        }

        internal TransformInspector()
            : base("TransformInspector")
        {
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawCopyToSystemBuffer();
        }

        private void OnPreSceneGUI()
        {
            base.OnSceneGUI();
        }

        protected override void OnHeaderGUI()
        {
            base.OnHeaderGUI();
        }

        private void DrawCopyToSystemBuffer()
        {
            var sContents = Contents.instance;
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button(sContents.contentCopyLocalPos, sContents.styleButtonLeft))
                {
                    var transform = target as Transform;
                    if (transform != null) GUIUtility.systemCopyBuffer = transform.localPosition.ToString();
                }
                if (GUILayout.Button(sContents.contentCopyGlobalPos, sContents.styleButtonMid))
                {
                    var transform = target as Transform;
                    if (transform != null) GUIUtility.systemCopyBuffer = transform.position.ToString();
                }
                if (GUILayout.Button(sContents.contentCopyLocalRotation, sContents.styleButtonMid))
                {
                    var transform = target as Transform;
                    if (transform != null) GUIUtility.systemCopyBuffer = transform.localRotation.ToEuler().ToString();
                }
                if (GUILayout.Button(sContents.contentCopyGlobalRotation, sContents.styleButtonMid))
                {
                    var transform = target as Transform;
                    if (transform != null) GUIUtility.systemCopyBuffer = transform.rotation.ToEuler().ToString();
                }
                if (GUILayout.Button(sContents.contentCopyScale, sContents.styleButtonMid))
                {
                    var transform = target as Transform;
                    if (transform != null) GUIUtility.systemCopyBuffer = transform.localScale.ToString();
                }
                if (GUILayout.Button(sContents.contentCopyNodePath, sContents.styleButtonRight))
                {
                    var transform = target as Transform;
                    string node = string.Empty;
                    CommonUtility.GetNodePath(transform, ref node);
                    GUIUtility.systemCopyBuffer = node;
                }
            }
            GUILayout.EndHorizontal();
        }
    }

    class Contents : Singleton<Contents>
    {
        public readonly GUIContent contentCopyLocalPos = new GUIContent("CLPos", "Copy local pos");
        public readonly GUIContent contentCopyGlobalPos = new GUIContent("CGPos", "Copy local pos");
        public readonly GUIContent contentCopyLocalRotation = new GUIContent("CLRotation", "Copy local rotation");
        public readonly GUIContent contentCopyGlobalRotation = new GUIContent("CGPos", "Copy local pos"); 
        public readonly GUIContent contentCopyScale = new GUIContent("CScale", "Copy local scale"); 
        public readonly GUIContent contentCopyNodePath = new GUIContent("CPath", "Copy node path"); 

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