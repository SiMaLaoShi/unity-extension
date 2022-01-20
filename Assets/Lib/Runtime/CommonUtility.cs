using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Lib.Runtime
{
    public partial class CommonUtility
    {
         /// <summary>
        ///     把一个字符串转换为bool
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool StringToBoolean(string val)
        {
            return val == "1" ? true : false;
        }

        /// <summary>
        ///     把一个bool值转换为字符串
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string BooleanToString(bool val)
        {
            return val ? "1" : "0";
        }

        /// <summary>
        ///     获取一个文件的md5值
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <returns>md5值</returns>
        public static string Md5(string fileName)
        {
            try
            {
                var file = new FileStream(fileName, FileMode.Open);
                MD5 md5 = new MD5CryptoServiceProvider();
                var retVal = md5.ComputeHash(file);
                file.Close();

                var sb = new StringBuilder();
                for (var i = 0; i < retVal.Length; i++) sb.Append(retVal[i].ToString("x2"));

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }

        /// <summary>
        ///     创建一个文件夹并删除
        /// </summary>
        /// <param name="dir">文件夹路径</param>
        /// <returns>DirectoryInfo</returns>
        public static DirectoryInfo Mkdir(string dir)
        {
            if (Directory.Exists(dir))
                Directory.Delete(dir, true);
            return Directory.CreateDirectory(dir);
        }

        /// <summary>
        ///     删除文件夹或者文件
        /// </summary>
        /// <param name="path">文件路径或者文件夹路径</param>
        public static void Delete(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }

        /// <summary>
        ///     创建一个文件
        /// </summary>
        /// <param name="file">文件路径</param>
        /// <returns>type FileInfo</returns>
        public static FileInfo Touch(string file)
        {
            Delete(file);
            var fileInfo = new FileInfo(file);
            return fileInfo;
        }

        /// <summary>
        ///     递归获取某个文件夹下所有文件
        /// </summary>
        /// <param name="files">存放文件路径的列表</param>
        /// <param name="dir">文件夹路径</param>
        /// <param name="pattern">文件类型</param>
        public static void RecursiveGetAllFile(ref List<string> files, string dir, string pattern = null)
        {
            files.AddRange(pattern == null
                ? Directory.GetFiles(dir)
                : Directory.GetFiles(dir, pattern, SearchOption.AllDirectories));

            var dirs = Directory.GetDirectories(dir);
            foreach (var d in dirs)
                RecursiveGetAllFile(ref files, d);
        }

        /// <summary>
        ///     非递归获取某个文件夹下所有文件
        /// </summary>
        /// <param name="files">存放文件路径的列表</param>
        /// <param name="dir">文件夹路径</param>
        /// <param name="pattern">文件类型</param>
        public static void NotRecursiveGetAllFile(ref List<string> files, string dir, string pattern = null)
        {
            var dirs = Directory.GetDirectories(dir, "*.*", SearchOption.AllDirectories);
            foreach (var path in dirs)
                files.AddRange(Directory.GetFiles(path, pattern ?? "*.*", SearchOption.AllDirectories));
        }

        /// <summary>
        ///     拷贝文件夹
        /// </summary>
        /// <param name="sourceDir">源文件夹</param>
        /// <param name="targetDir">目标文件夹</param>
        /// <param name="pattern">文件匹配模式</param>
        /// <param name="delete">是否删除原来的目标文件夹</param>
        /// <param name="ignore">忽略文件类型</param>
        public static void CopyDir(string sourceDir, string targetDir, string pattern = null, bool delete = false,
            List<string> ignore = null)
        {
            if (!Directory.Exists(sourceDir))
                return;
            if (delete)
                Mkdir(targetDir);
            else if (!Directory.Exists(targetDir))
                Directory.CreateDirectory(targetDir);

            var dirs = Directory.GetDirectories(sourceDir, "*.*", SearchOption.AllDirectories);
            for (var i = 0; i < dirs.Length; i++)
            {
                if (!Directory.Exists(dirs[i].Replace(sourceDir, targetDir)))
                    Directory.CreateDirectory(dirs[i].Replace(sourceDir, targetDir));
            }

            var files = new List<string>();
            RecursiveGetAllFile(ref files, sourceDir, pattern);
            for (var i = 0; i < files.Count; i++)
            {
                if (IsIgnore(ignore, files[i]))
                    continue;
                File.Copy(files[i], files[i].Replace(sourceDir, targetDir), true);
            }
        }

        public static bool IsIgnore(List<string> ignore, string file)
        {
            if (ignore == null)
                return false;
            foreach (var s in ignore)
                if (file.EndsWith(s))
                    return true;

            return false;
        }
    }
}