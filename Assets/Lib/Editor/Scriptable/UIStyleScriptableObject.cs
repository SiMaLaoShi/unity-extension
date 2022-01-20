using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lib.Editor.Scriptable
{
    public class UIStyleScriptableObject : BaseScriptable<UIStyleScriptableObject>
    {
        public List<TextStyle> TextStyleList = new List<TextStyle>();
        public List<int> FontSizeList = new List<int>();
        public List<ButtonStyle> ButtonStyleList = new List<ButtonStyle>();
    }
    
    [Serializable]
    public class TextStyle
    {
        public string Desc = String.Empty;
        public Font Font;
        public int FontSize = 20;
        public Color FontColor;
        public TextAnchor TextAnchor = TextAnchor.MiddleCenter; 
        
        public bool IsOutline = false;
        public Color OutlineColor;
        public Vector2 OutlineDistance;
        public bool IsSizeFitter = false;
        public ContentSizeFitter.FitMode FitMode;
    }
    
    [Serializable]
    public class ButtonStyle
    {
        public int FontSize;
        public Color FontColor;
        public bool IsOutline;
        public Color OutLineColor;
    }
}