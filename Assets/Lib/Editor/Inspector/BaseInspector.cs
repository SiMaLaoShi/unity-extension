using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using UEditor = UnityEditor.Editor;

public abstract class BaseInspector : Editor {
			// empty array for invoking methods using reflection
		private static readonly object[] EMPTY_ARRAY = new object[0];

		private static readonly Dictionary<string, MethodInfo> decoratedMethods = new Dictionary<string, MethodInfo>();

		private static readonly Assembly editorAssembly = Assembly.GetAssembly(typeof(UEditor));

		protected BaseInspector(string editorTypeName)
		{
			decoratedEditorType = editorAssembly.GetTypes().FirstOrDefault(t => t.Name == editorTypeName);
			Init();

			// Check CustomEditor types.
			Type originalEditedType = GetCustomEditorType(decoratedEditorType);
			if (originalEditedType != editedObjectType)
			{
				throw new ArgumentException(
					string.Format("Type {0} does not match the editor {1} type {2}", editedObjectType, editorTypeName,
						originalEditedType));
			}
		}

		protected UEditor EditorInstance
		{
			get
			{
				if (editorInstance == null && targets != null && targets.Length > 0)
				{
					editorInstance = CreateEditor(targets, decoratedEditorType);
				}

				if (editorInstance == null) { Debug.LogError("Could not create editor !"); }

				return editorInstance;
			}
		}

		private static Type GetCustomEditorType(Type type)
		{
			const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;

			CustomEditor[] attributes = type.GetCustomAttributes(typeof(CustomEditor), true) as CustomEditor[];
			if (null != attributes)
			{
				FieldInfo field = attributes.Select(editor => editor.GetType().GetField("m_InspectedType", flags)).First();
				if (null != field)
					return field.GetValue(attributes[0]) as Type;
			}
			return null;
		}

		private void Init() { editedObjectType = GetCustomEditorType(GetType()); }

		protected virtual void OnEnable()
		{
			if (editorInstance) { CallInspectorMethod("OnEnable"); }
		}

		protected virtual void OnDisable()
		{
			if (editorInstance != null) { DestroyImmediate(editorInstance); }
		}

		protected void CallFieldMethod(string fieldName, string methodName, Type[] types, params object[] parameters)
		{
			Type type = EditorInstance.GetType();
			FieldInfo fieldInfo = type.GetField(fieldName,
				BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			type = fieldInfo.FieldType;
			MethodInfo methodInfo = type.GetMethod(methodName, types);
			methodInfo.Invoke(fieldInfo.GetValue(EditorInstance), parameters);
		}

		protected void SetFieldValue(string fieldName, string valueName, object value)
		{
			Type type = EditorInstance.GetType();
			FieldInfo fieldInfo = type.GetField(fieldName,
				BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			type = fieldInfo.FieldType;
			FieldInfo fieldInfo2 = type.GetField(valueName,
				BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			fieldInfo2.SetValue(fieldInfo.GetValue(EditorInstance), value);
		}

		/// <summary>
		///     Delegates a method call with the given name to the decorated editor instance.
		/// </summary>
		protected void CallInspectorMethod(string methodName, UEditor editor)
		{
			MethodInfo method = null;

			// Add MethodInfo to cache
			if (!decoratedMethods.ContainsKey(methodName))
			{
				const BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic |
				                           BindingFlags.Public;

				method = decoratedEditorType.GetMethod(methodName, flags);

				if (method != null) { decoratedMethods[methodName] = method; }
				else { Debug.LogError(string.Format("Could not find method {0}", (MethodInfo)null)); }
			}
			else { method = decoratedMethods[methodName]; }

			if (null != method)
				method.Invoke(editor, EMPTY_ARRAY);
		}

		protected void CallInspectorMethod(string methodName) { CallInspectorMethod(methodName, editorInstance); }

		protected virtual void OnSceneGUI()
		{
			if (editorInstance) { CallInspectorMethod("OnSceneGUI"); }
		}

		protected override void OnHeaderGUI()
		{
			if (editorInstance) { CallInspectorMethod("OnHeaderGUI"); }
		}

		public override void OnInspectorGUI() { EditorInstance.OnInspectorGUI(); }

		public override void DrawPreview(Rect previewArea) { EditorInstance.DrawPreview(previewArea); }

		public override string GetInfoString()
		{
			return EditorInstance.GetInfoString();
		}

		public override GUIContent GetPreviewTitle()
		{
			return EditorInstance.GetPreviewTitle();	
		}

		public override bool HasPreviewGUI()
		{
			return EditorInstance.HasPreviewGUI();	
		} 

		public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
		{
			EditorInstance.OnInteractivePreviewGUI(r, background);
		}

		public override void OnPreviewGUI(Rect r, GUIStyle background) { EditorInstance.OnPreviewGUI(r, background); }

		public override void OnPreviewSettings() { EditorInstance.OnPreviewSettings(); }

		public override void ReloadPreviewInstances() { EditorInstance.ReloadPreviewInstances(); }

		public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
		{
			return EditorInstance.RenderStaticPreview(assetPath, subAssets, width, height);
		}
			

		public override bool RequiresConstantRepaint()
		{
			return EditorInstance.RequiresConstantRepaint();;
		}

		public override bool UseDefaultMargins()
		{
			return EditorInstance.UseDefaultMargins();	
		} 

		#region Editor Fields

		/// <summary>
		///     Type object for the internally used (decorated) editor.
		/// </summary>
		private readonly Type decoratedEditorType;

		/// <summary>
		///     Type object for the object that is edited by this editor.
		/// </summary>
		private Type editedObjectType;

		private UEditor editorInstance;

		#endregion
}
