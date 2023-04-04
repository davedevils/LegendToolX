using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altra_Tool
{
    public class IniReader
    {
        public string ReadIconName(string id, string filepath)
        {
            if (!File.Exists(filepath))
            {
                return "";
            }

            string readText = File.ReadAllText(filepath, Encoding.GetEncoding("big5"));
            readText = readText.Replace(System.Environment.NewLine, string.Empty);

            if (readText[0] != '|')
                return "";

            string Filename = Path.GetFileName(filepath);
            string PathFile = Path.GetDirectoryName(filepath);

            List<String> ParsedList = SplitStrings(readText, '|');
            int NumberPerLine = int.Parse(ParsedList[1]);

            for (var i = 2; i < ParsedList.Count;)
            {

                if (i + NumberPerLine < ParsedList.Count)
                {
                    if(id == ParsedList[i + 0])
                     return ParsedList[i + 1];

                }

                i = i + NumberPerLine;
            }


            return "";
        }

        public static List<String> SplitStrings(String str, char dl)
        {
            String word = "";

            // adding delimiter character  
            // at the end of 'str' 
            str = str + dl;

            // length of 'str' 
            int l = str.Length;

            // traversing 'str' from left to right 
            List<String> substr_list = new List<String>();
            for (int i = 0; i < l; i++)
            {

                // if str[i] is not equal to the delimiter 
                // character then accumulate it to 'word' 
                if (str[i] != dl)
                {
                    word = word + str[i];
                }
                else
                {

                    // if 'word' is not an empty String, 
                    // then add this 'word' to the array 
                    // 'substr_list[]' 
                    //if ((int)word.Length != 0)
                    if (substr_list.Count > 0 || (int)word.Length != 0)
                    {
                        substr_list.Add(word);
                    }

                    // reset 'word' 
                    word = "";
                }
            }

            // return the splitted Strings 
            return substr_list;
        }
    }

}
