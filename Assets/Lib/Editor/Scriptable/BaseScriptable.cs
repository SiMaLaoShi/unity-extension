using UnityEditor;
using UnityEngine;

namespace Lib.Editor.Scriptable
{
	public class BaseScriptable<T>: ScriptableObject where T: ScriptableObject
	{
		private static T instance;
		public static T Instance
		{
			get
			{
				if (instance == null)
				{
					var name = typeof(T).Name;
					var path = string.Format("Assets/{0}.asset", name);
					instance = AssetDatabase.LoadAssetAtPath<T>(path);
					if (instance == null)
					{
						instance = CreateInstance<T>();
						instance.name = name;
						AssetDatabase.CreateAsset(instance, path);
					}
				}
				return instance;
			}
		}
	}
}