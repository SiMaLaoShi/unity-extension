using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Lib.Editor
{
    public partial class CommonUtility 
    {
        [MenuItem("Assets/PrintAssets/Print Filesize")]
        public static void PrintFileSize()
        {
            var paths = GetSelectionAssetPaths(true);
            foreach (var path in paths)
            {
                var filePath = Path.Combine(System.Environment.CurrentDirectory, path);
                if (Directory.Exists(filePath))
                {
                    List<FileInfo> infos = new List<FileInfo>();
                    EditorUtil.GetAllFile(filePath, ref infos);
                    foreach (var info in infos)
                    {
                        if (info.Extension == ".meta")
                            continue;
                        FormatPrint(info, path);
                    }
                    continue;
                }
                FileInfo file = new FileInfo(filePath);
                FormatPrint(file, path);
            }
        }
        
        static void FormatPrint(FileInfo file, string path)
        {
            if (file.Length / 1024 > 100)
                Debug.Log(path + "<color=#ff0000>【" + file.Length / 1024 + "kb】</color>");
            else
                Debug.Log(path + "<color=#00ff00>【" + file.Length / 1024 + "kb】</color>");
        }
    }
}