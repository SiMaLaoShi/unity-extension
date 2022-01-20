using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Lib.Runtime.Extensions
{
    public static partial class Extension 
    {
        public static RaycastResult[] RaycastAll( this EventSystem es , PointerEventData eventData )
        {
            List<RaycastResult> res = new List<RaycastResult>();
            es.RaycastAll(eventData, res);
            return res.ToArray();
        }
    }
}