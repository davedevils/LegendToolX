using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslateMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                Console.WriteLine("--------------------------------------");
                Console.WriteLine("| Translate File maker By DaveDevils |");
                Console.WriteLine("--------------------------------------");
                foreach (var arg in args)
                {
                    switch(ReadFile(arg))
                    {
                        case -1:
                            Console.WriteLine(arg + " : File don't exist.");
                            break;
                        case 0:
                            Console.WriteLine(arg + " : This is not a x-legend file.");
                            break;
                        case 5:
                            Console.WriteLine(arg + " : converted to T_ .");
                            break;
                        default:
                            Console.WriteLine(arg +" : General ERROR");
                            break;
                    }
                }
            }
        }

        static int ReadFile(string filepath)
        {
            if (!File.Exists(filepath))
            {
                return -1;
            }

            string readText = File.ReadAllText(filepath, GetEncodingFile(filepath));
            readText = readText.Replace(System.Environment.NewLine, string.Empty);

            if (readText[0] != '|')
                return 0;

            string Filename = Path.GetFileName(filepath);
            string PathFile = Path.GetDirectoryName(filepath);

            if (Filename.Split('_').Count() > 1)
                Filename = Filename.Split('_')[1];

            //Split 
            List <String> ParsedList = splitStrings(readText, '|');

            int NumberPerLine = int.Parse(ParsedList[1]);

            string TranslateFileLink = PathFile + "\\T_" + Filename;

            if (File.Exists(TranslateFileLink))
            {
                File.Delete(TranslateFileLink);
            }

            using (System.IO.StreamWriter NewFile = new System.IO.StreamWriter(TranslateFileLink))
            {
                NewFile.WriteLine(ParsedList[0].ToString() + "" + HeaderTranslate(Filename));
                NewFile.WriteLine(SubheaderTranslate(Filename));
                for (var i = 2; i < ParsedList.Count;)
                {
                    if (i + NumberPerLine < ParsedList.Count)
                    {
                        // Console.WriteLine("text is {0}", ParsedList[i + 2]);

                            string FinalString = "";
                            foreach (int Value in CreateTranslateLine(Filename))
                            {
                                FinalString += ParsedList[i + Value] + "|";
                            }
                            NewFile.WriteLine(FinalString);

                    }

                    i = i + NumberPerLine;
                }
            }

            return 5;
        }

        static string HeaderTranslate(string value)
        {
            switch (value.ToLower())
            {
                case "mission.ini":
                    return "|4|";
                default:
                    return "|3|";
            }
        }

        static string SubheaderTranslate(string value)
        {
            switch (value.ToLower())
            {
                case "mission.ini":
                    return "1|3|15|33|";
                default:
                    return "|3|";
            }
        }

        static List<int> CreateTranslateLine(string value)
        {
            switch (value.ToLower())
            {
                case "mission.ini":
                    return new List<int>(){ 0,2, 14, 32 };
                default:
                    return new List<int>(){ 0, 2, 14, 32 };
            }
        }

        static System.Text.Encoding GetEncodingFile(string filePath)
        {
            System.Text.Encoding enc = null;
            System.IO.FileStream file = new System.IO.FileStream(filePath,
                FileMode.Open, FileAccess.Read, FileShare.Read);
            if (file.CanSeek)
            {
                byte[] bom = new byte[4]; // Get the byte-order mark, if there is one 
                file.Read(bom, 0, 4);
                if ((bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) || // utf-8 
                    (bom[0] == 0xff && bom[1] == 0xfe) || // ucs-2le, ucs-4le, and ucs-16le 
                    (bom[0] == 0xfe && bom[1] == 0xff) || // utf-16 and ucs-2 
                    (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff)) // ucs-4 
                {
                    enc = System.Text.Encoding.Unicode;
                }
                else
                {
                    enc = Encoding.GetEncoding("big5");
                }

                // Now reposition the file cursor back to the start of the file 
                file.Seek(0, System.IO.SeekOrigin.Begin);
            }
            else
            {
                // The file cannot be randomly accessed, so you need to decide what to set the default to 
                // based on the data provided. If you're expecting data from a lot of older applications, 
                // default your encoding to Encoding.ASCII. If you're expecting data from a lot of newer 
                // applications, default your encoding to Encoding.Unicode. Also, since binary files are 
                // single byte-based, so you will want to use Encoding.ASCII, even though you'll probably 
                // never need to use the encoding then since the Encoding classes are really meant to get 
                // strings from the byte array that is the file. 

                enc = Encoding.GetEncoding("big5");
            }
            file.Close();
            return enc;
        }

        static List<String> splitStrings(String str, char dl)
        {
            String word = "";

            // to count the number of split Strings 
            int num = 0;

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
                    if(substr_list.Count > 0 || (int)word.Length != 0)
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
