using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lib.Runtime.Extensions
{
    public static partial class Extension
    {
        public static void AddOptionDataArray(this Dropdown dropdown, Dropdown.OptionData[] options)
        {
            dropdown.AddOptions(new List<Dropdown.OptionData>(options));
        }
        
        public static void AddOptionSpriteArray(this Dropdown dropdown, Sprite[] options)
        {
            dropdown.AddOptions(new List<Sprite>(options));
        }

        public static void AddOptionStringArray(this Dropdown dropdown, string[] options)
        {
            dropdown.AddOptions(new List<string>(options));
        }

        public static void SetOptionDataArray(this Dropdown dropdown , Dropdown.OptionData[] options )
        {
            dropdown.options = new List<Dropdown.OptionData>(options);
        }

        public static Dropdown.OptionData[] GetOptionDataArray(this Dropdown dropdown )
        {
            return dropdown.options.ToArray();
        }
    }
}