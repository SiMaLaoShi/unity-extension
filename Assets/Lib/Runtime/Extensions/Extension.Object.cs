using System;
using System.Collections.Generic;

namespace Lib.Runtime.Extensions
{
    public static partial class Extension
    {
        public static int ToInt(this object o)
        {
            return Convert.ToInt32(o);
        }

        public static uint ToUint(this object o)
        {
            return Convert.ToUInt32(o);
        }

        public static float ToFloat(this object o)
        {
            return Convert.ToSingle(o);
        }

        public static long ToLong(this object o)
        {
            return Convert.ToInt64(o);
        }
    }
}