using System;
using System.Collections.Generic;

namespace Module.Utility
{
    public class RichTextFlag
    {
        public int StartIndex;
        public int EndIndex;
        public string RichStringBegin=String.Empty;
        public string RichStringEnd=String.Empty;
        public bool Finish=false;
    }
    public class RichTextEx
    {
        public static bool TryFinishFlag(string str, List<RichTextFlag> flagList,int endIndex)
        {
            if (flagList == null)
            {
                return false;
            }
            // var matchStr = str.RemoveString("/").RemoveString(">");
            // for (int i = flagList.Count - 1; i >= 0; i--)
            // {
            //     var data = flagList[i];
            //     if (data.Finish)
            //     {
            //         continue;
            //     }
            //
            //     if (data.RichStringBegin.StartsWith(matchStr))
            //     {
            //         data.Finish = true;
            //         data.RichStringEnd = str;
            //         data.EndIndex = endIndex;
            //         return true;
            //     }
            // }
            return false;
        }
        public static bool IsFlagStart(string str)
        {
            if (str.StartsWith("<") && str.EndsWith(">") && !str.StartsWith("</"))
            {
                return true;
            }
            return false;
        }

        public static bool IsFlagEnd(string str)
        {
            if (str.EndsWith(">") && str.StartsWith("</"))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取字符串中第几个非富文本字符在原字符串中的index
        /// </summary>
        /// <param name="str"></param>
        /// <param name="validCharCount"></param>
        /// <returns></returns>
        public static int GetSourceStrIndex(string str, int validCharCount)
        {
            var curCollectionIndex = 0;
            var validLength = 0;
            var fullIndex = 0;
            // var lCollection = StringEx.RichTextRegex.Matches(str);
            // for (int i = 0; i < str.Length; i++)
            // {
            //     if (curCollectionIndex<lCollection.Count&&i >= lCollection[curCollectionIndex].Index)
            //     {
            //         if (i >= lCollection[curCollectionIndex].Index + lCollection[curCollectionIndex].Value.Length-1)
            //         {
            //             curCollectionIndex++;
            //         }
            //     }
            //     else
            //     {
            //         validLength++;
            //         if (validLength == validCharCount)
            //         {
            //             fullIndex = i+1;
            //             break;
            //         }
            //     }
            // }
            return fullIndex;
        }
    }
}