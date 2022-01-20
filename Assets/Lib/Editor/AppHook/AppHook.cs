using Lib.Editor.Scriptable;
using PlayHooky;
using UnityEditor;
using UnityEngine;

public class AppHook : MonoBehaviour {
	
	public static string oldStreamingAssetsPath = Application.streamingAssetsPath;
	public static string oldPersistentDataPath = Application.persistentDataPath;
	public static string streamingAssetsPath
	{
		get
		{
			if (!GlobalScriptableObject.Instance.isHookApplication)
				return oldStreamingAssetsPath;
			var p = GlobalScriptableObject.Instance.strRemoteStreamingAssetsPath;
			return p == "" ? oldStreamingAssetsPath : p;
		}
	}

	public static string persistentDataPath
	{
		get
		{
			if (!GlobalScriptableObject.Instance.isHookApplication)
				return oldPersistentDataPath;
			var p = GlobalScriptableObject.Instance.strRemotePersistentDataPath;
			return p == "" ? oldPersistentDataPath : p;
		}
	}
	
#if UNITY_5 || UNITY_2017_1_OR_NEWER
	//这个属性在场景加载后直接启动我们的方法
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
#endif
	public static void OnStartGame()
	{
		EditorApplication.playModeStateChanged += OnPlayerModeStateChanged;
		if (!GlobalScriptableObject.Instance.isHookApplication)
			return;
		HookManager manager = new HookManager();
		//这里Hook streamingAssetsPath，因为本身自动属性就是一个方法，直接用这个替换就行
		if (GlobalScriptableObject.Instance.isHookStreamingAssetsPath)
			manager.Hook(typeof(Application).GetMethod("get_streamingAssetsPath"), typeof(AppHook).GetMethod("get_streamingAssetsPath"));
		if (GlobalScriptableObject.Instance.isHookPersistentDataPath)
			manager.Hook(typeof(Application).GetMethod("get_persistentDataPath"), typeof(AppHook).GetMethod("get_persistentDataPath")); Debug.Log("Application.streamingAssetsPath:" + Application.streamingAssetsPath);
		Debug.Log("Application.persistentDataPath:" + Application.persistentDataPath);
		Debug.Log("Application.streamingAssetsPath:" + Application.streamingAssetsPath);
	} 
	
	static void OnPlayerModeStateChanged(PlayModeStateChange playModeStateChange)
	{
	}
}
