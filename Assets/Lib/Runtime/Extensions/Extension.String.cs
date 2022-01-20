using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lib.Runtime.Extensions
{
    public static partial class Extension
    {
        public static bool EndWishEx(this string str, string a, string b)
        {
            var ap = a.Length - 1;
            var bp = b.Length - 1;

            while (ap >= 0 && bp >= 0 && a[ap] == b[bp])
            {
                ap--;
                bp--;
            }

            return bp < 0 && a.Length >= b.Length || ap < 0 && b.Length >= a.Length;
        }

        public static bool StartsWithEx(this string str, string a, string b)
        {
            var aLen = a.Length;
            var bLen = b.Length;
            var ap = 0;
            var bp = 0;

            while (ap < aLen && bp < bLen && a[ap] == b[bp])
            {
                ap++;
                bp++;
            }

            return bp == bLen && aLen >= bLen || ap == aLen && bLen >= aLen;
        }
        
        public static List<T> ToList<T>(this string o, char splitChar) 
        {
            if (string.IsNullOrEmpty(o)) return null;
            var list = new List<T>();
            var strs = o.Split(splitChar);
            foreach(var str in strs)
            {
                list.Add((T)Convert.ChangeType(str, typeof(T)));
            }
            return list;
        } 
        
        public static Vector3 ToVec3(this string input, char splitChar)
        {
            if (string.IsNullOrEmpty(input)) return Vector3.zero;
            string[] strs = input.Split(splitChar);
            return new Vector3(float.Parse(strs[0]), float.Parse(strs[1]), float.Parse(strs[2]));
        }
        
        public static Vector3Int ToVec3Int(this string input, char splitChar)
        {
            if (string.IsNullOrEmpty(input)) return Vector3Int.zero;
            string[] strs = input.Split(splitChar);
            return new Vector3Int(int.Parse(strs[0]), int.Parse(strs[1]), int.Parse(strs[2]));
        }
    }
}