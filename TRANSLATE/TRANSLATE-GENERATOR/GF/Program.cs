using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TranslateMaker
{
    class Program
    {
        static void Main(string[] args)
        {

            string[] FilenameCheck = Directory.GetFiles("db\\");


            Console.WriteLine("---------------------------");
            Console.WriteLine("| Translate File Generator|");
            Console.WriteLine("--------------------------");

            foreach (var arg in FilenameCheck)
            {
                if (arg.Contains("db\\C_")
                 || arg.Contains("db\\S_")
                 || arg.Contains("db\\c_")
                 || arg.Contains("db\\s_"))
                {

                    switch (ReadFile(arg))
                    {
                        case -1:
                            Console.WriteLine(arg + " : file don't exist.");
                            break;
                        case 0:
                            Console.WriteLine(arg + " : is not a x-legend file.");
                            break;
                        case 1:
                            Console.WriteLine(arg + " : have no translate file.");
                            break;
                        case 5:
                            Console.WriteLine(arg + " : patched file with new translate.");
                            break;
                        default:
                            Console.WriteLine(arg + " : General ERROR");
                            break;
                    }
                    Console.WriteLine("-----------------------");
                }
            }
            int seconde = 50;
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.Write($"Program will close on " +seconde.ToString()+" seconde");
            var delay = Task.Delay(TimeSpan.FromSeconds(seconde));
            
            while (!delay.IsCompleted)
            {
                seconde--;
                Thread.Sleep(TimeSpan.FromSeconds(1));
                Console.SetCursorPosition(Console.CursorLeft - 8, Console.CursorTop);
                if (seconde < 9)
                {
                    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                }
                else
                {
                    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                }

                Console.Write($"{seconde}");
                Console.SetCursorPosition(Console.CursorLeft + 8, Console.CursorTop);
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

            List<String> ParsedList = splitStrings(readText, '|');
            int NumberPerLine = int.Parse(ParsedList[1]);

            List<int> RemplaceList = CreateTranslateLine(Filename);
            List<int> EmptyList = new List<int>() { 0 };

            if (RemplaceList == EmptyList)
            {
                //Generate Pos list
            }

            //Create Translate File

            PathFile = PathFile.Remove(PathFile.Length - 2, 2);
            string TranslateFileLink = PathFile + "Translate\\T_" + Filename;

            return 5;
        }

        public struct StructTranslate1
        {
            public string id;
            public string translate1;
        }

        public struct StructTranslate2
        {
            public string id;
            public string translate1;
            public string translate2;
        }

        static List<int> CreateTranslateLine(string value)
        {
            switch (value.ToLower())
            {
                case "textindex.ini":
                    return new List<int>(){ 2 };
                case "battlefield.ini":
                    return new List<int>() { 1 };
                case "beaststower.ini":
                    return new List<int>() { 2 };
                case "collection.ini":
                    return new List<int>() { 7 };
                case "dialogue.ini":
                    return new List<int>() { 1 };
                case "mission.ini":
                    return new List<int>() { 1 };
                case "monster.ini":
                    return new List<int>() { 2 };
                case "npc.ini":
                    return new List<int>() { 2 };
                case "vip.ini":
                    return new List<int>() { 1 };
                case "pointability.ini":
                    return new List<int>() { 1 };

                case "itemcombo.ini":
                    return new List<int>() { 2 };
                case "item.ini":
                    return new List<int>() { 9, 92 };
                case "itemmall.ini":
                    return new List<int>() { 9, 92 };

                case "spell.ini":
                    return new List<int>() { 11, 59 };


                case "title.ini":
                    return new List<int>() { 1, 5 };
                    





                default:
                    return new List<int>(){0};
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
