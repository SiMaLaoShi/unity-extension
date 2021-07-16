using UnityEditor;
using UnityEngine;

public class ConfigAsset : ScriptableObject
{
    private static ConfigAsset instance;
    public string NotePad = @"notepad.exe";
    public string NotePadPPPath = @"C:\Program Files (x86)\Notepad++\notepad++.exe";
    public string SublimePath = @"C:\Program Files\Sublime Text 3\sublime_text.exe";
    public bool useAni = false;
    public bool isHookApplication = false;
    public bool isHookStreamingAssetsPath = false;
    public string szRemoteStreamingAssetsPath = "";
    public bool isHookPersistentDataPath = false;
    public string szRemotePersistentDataPath = "";
    public static ConfigAsset Instance
    {
        get
        {
            if (instance == null)
            {
                instance = AssetDatabase.LoadAssetAtPath<ConfigAsset>("Assets/ConfigAsset.asset");
                if (instance == null)
                {
                    instance = CreateInstance<ConfigAsset>();
                    instance.name = "ConfigAsset";
                    AssetDatabase.CreateAsset(instance, "Assets/ConfigAsset.asset");
                }
            }

            return instance;
        }
    }
}