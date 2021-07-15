using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

/// <summary>
///     todo 实现走配置直接打开不需要代码
/// </summary>
public class OpenWay : Editor
{
    [MenuItem("Assets/打开方式/NotePad++")]
    private static void NotePadPlusPlusRun()
    {
        var assetPaths = GameExtension.GetSelectionAssetPaths(true);
        OsRun(string.Join(" ", assetPaths.ToArray()), ConfigAsset.Instance.NotePadPPPath);
    }

    [MenuItem("Assets/打开方式/Sublime Text")]
    private static void SublimeTextRun()
    {
        var assetPaths = GameExtension.GetSelectionAssetPaths(true);
        OsRun(string.Join(" ", assetPaths.ToArray()), ConfigAsset.Instance.SublimePath);
    }

    [MenuItem("Assets/打开方式/NotePad")]
    private static void NotePadRun()
    {
        var assetPaths = GameExtension.GetSelectionAssetPaths(true);
        OsRun(string.Join(" ", assetPaths.ToArray()), ConfigAsset.Instance.NotePad);
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
}