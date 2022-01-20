using System;
using System.Diagnostics;
using Lib.Editor.Scriptable;
using UnityEditor;
using UnityEngine;

namespace Lib.Editor
{
    public partial class CommonUtility 
    {
#if UNITY_EDITOR_WIN
        [MenuItem("Assets/Open By/NotePad++")]
        private static void NotePadPlusPlusRun()
        {
            var assetPaths = CommonUtility.GetSelectionAssetPaths(true);
            OsRun(string.Join(" ", assetPaths.ToArray()), GlobalScriptableObject.Instance.strNotePadPpPath);
        }

        [MenuItem("Assets/Open By/Sublime Text")]
        private static void SublimeTextRun()
        {
            var assetPaths = CommonUtility.GetSelectionAssetPaths(true);
            OsRun(string.Join(" ", assetPaths.ToArray()), GlobalScriptableObject.Instance.strSublimePath);
        }

        [MenuItem("Assets/Open By/NotePad")]
        private static void NotePadRun()
        {
            var assetPaths = CommonUtility.GetSelectionAssetPaths(true);
            OsRun(string.Join(" ", assetPaths.ToArray()), GlobalScriptableObject.Instance.strNotePad);
        }
    
        [MenuItem("Assets/Open By/NotePad打开.Meta(选一个)")]
        private static void OpenMeta()
        {
            var guids = Selection.assetGUIDs;
            if (guids.Length == 1)
                OsRun(Environment.CurrentDirectory + "/" + AssetDatabase.GUIDToAssetPath(guids[0]) + ".meta", GlobalScriptableObject.Instance.strNotePad);
        }

        private static void OsRun(string args, string exePath)
        {
            var workDirectory =
                Application.dataPath.Remove(Application.dataPath.LastIndexOf("/Assets", StringComparison.Ordinal));
            Process.Start(new ProcessStartInfo
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                FileName = exePath,
                Arguments = args,
                WorkingDirectory = workDirectory
            });
        }
#endif
    }
}