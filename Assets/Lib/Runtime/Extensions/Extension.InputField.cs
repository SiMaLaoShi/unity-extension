using UnityEngine.UI;

namespace Lib.Runtime.Extensions
{
    public static partial class Extension
    {
        public static int GetInputWordsLength(this InputField inputField)
        {

            string text = inputField.text;
            int length = 0;
            int maxAsciiEnc = 255;
            for(int i = 0; i < text.Length; i ++)
            {
                if (text[i] > maxAsciiEnc)
                {
                    length = length + 2;
                }
                else
                {
                    length = length + 1;
                }
            }

            return length;
        }
    }
}