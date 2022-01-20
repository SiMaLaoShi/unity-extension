using UnityEngine;

namespace Lib.Runtime.Extensions
{
    public static partial class Extension 
    {
        public static string ToIntStr(this Vector3 vec, string splitChar)
        {
            return (int)vec.x + splitChar + (int)vec.y + splitChar + (int)vec.z;
        }
    }
}