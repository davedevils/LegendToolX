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
            List<string> FilenameCheck = new List<string>();

            FilenameCheck.Add("C_Achievement.ini");
            FilenameCheck.Add("C_Activity.ini");

            FilenameCheck.Add("C_Battlefield.ini");
            FilenameCheck.Add("C_BeastsTower.ini");

            FilenameCheck.Add("C_Classes.ini");
            FilenameCheck.Add("C_Collection.ini");

            FilenameCheck.Add("C_Dialogue.ini");
            FilenameCheck.Add("C_DynamicEvent.ini");

            FilenameCheck.Add("C_Elf.ini");
            FilenameCheck.Add("C_ElfCollect.ini");
            FilenameCheck.Add("C_ElfKing.ini");
            FilenameCheck.Add("C_ElfRacing.ini");
            FilenameCheck.Add("C_ElfTeamFight.ini");
            FilenameCheck.Add("C_ElfTemple.ini");
            FilenameCheck.Add("C_ElfTrain.ini");

            FilenameCheck.Add("C_Enchant.ini");

            FilenameCheck.Add("C_EquipSet.ini");
            FilenameCheck.Add("C_Exam.ini");

            FilenameCheck.Add("C_FamilyTree.ini");
            FilenameCheck.Add("C_Festival.ini");

            FilenameCheck.Add("C_Item.ini");
            FilenameCheck.Add("C_ItemCombo.ini");
            FilenameCheck.Add("C_ItemMall.ini");

            FilenameCheck.Add("C_MentorshipInstance.ini");
            FilenameCheck.Add("C_Mission.ini");
            FilenameCheck.Add("C_Monster.ini");

            FilenameCheck.Add("C_Node.ini");
            FilenameCheck.Add("C_Npc.ini");

            FilenameCheck.Add("C_PointAbility.ini");

            FilenameCheck.Add("C_Race.ini");
            FilenameCheck.Add("C_RaceGroup.ini");
            FilenameCheck.Add("C_RainbowEvent.ini");
            FilenameCheck.Add("C_RecommendEvents.ini");
            FilenameCheck.Add("C_RideCombo.ini");

            FilenameCheck.Add("C_Spell.ini");

            FilenameCheck.Add("C_TextIndex.ini");
            FilenameCheck.Add("C_Title.ini");

            FilenameCheck.Add("C_VIP.ini");

            Console.WriteLine("-----------------------");
            Console.WriteLine("| Translate File maker |");
            Console.WriteLine("-----------------------");

            foreach (var arg in FilenameCheck)
            {
                string filepath = "db\\"+arg;
                 switch(ReadFile(filepath))
                 {
                        case -1:
                            Console.WriteLine(filepath + " : file don't exist.");
                            break;
                        case 0:
                            Console.WriteLine(filepath + " : is not a x-legend file.");
                            break;
                        case 1:
                            Console.WriteLine(filepath + " : have no translate file.");
                            break;
                         case 5:
                            Console.WriteLine(filepath + " : patched file with new translate.");
                            break;
                        default:
                            Console.WriteLine(filepath + " : General ERROR");
                            break;
                 }
                Console.WriteLine("-----------------------");
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

            //Split 
            List <String> ParsedList = splitStrings(readText, '|');

            int NumberPerLine = int.Parse(ParsedList[1]);

            PathFile = PathFile.Remove(PathFile.Length - 2, 2);
            string TranslateFileLink = PathFile + "Translate\\T_" + Filename;

            if (!File.Exists(TranslateFileLink))
            {
                Console.WriteLine(TranslateFileLink + " translate file not found");
                return 1;
            }

            //Split Translate
            string ReadTranslate = File.ReadAllText(TranslateFileLink, GetEncodingFile(TranslateFileLink));
            List<String> ParsedTranslateList = splitStrings(ReadTranslate, '|');


            // If One Translate Row
            if (Filename == "Item.ini" ||
                Filename == "ItemMall.ini" ||
                Filename == "Spell.ini" ||
                Filename == "Title.ini")
            {
                //Now Store on memory translate
                int NumberPerLineTranslate = 2;
                List<StructTranslate2> TranslateStored = new List<StructTranslate2>();
                for (var i = 0; i < ParsedTranslateList.Count;)
                {

                    if (i + NumberPerLineTranslate < ParsedTranslateList.Count)
                    {
                        StructTranslate2 newTrad;
                        newTrad.id = ParsedTranslateList[i + 0].Replace(System.Environment.NewLine, string.Empty);
                        newTrad.translate1 = ParsedTranslateList[i + 1];
                        newTrad.translate2 = ParsedTranslateList[i + 2];
                        TranslateStored.Add(newTrad);

                    }

                    i = i + NumberPerLineTranslate;
                }

                //Now we read original file

                if (File.Exists(filepath + ".bak"))
                {
                    File.Delete(filepath + ".bak");
                }

                if (File.Exists(filepath))
                {
                    System.IO.File.Move(filepath, filepath + ".bak");
                }

                //readText - Full data
                //ParsedList - List of |
                //NumberPerLine - number of while

                //And we patch it.
                using (System.IO.StreamWriter NewFile = new StreamWriter(filepath, false, Encoding.GetEncoding("big5")))//new System.IO.StreamWriter(filepath))
                {
                    List<int> RemplaceText = CreateTranslateLine(Filename);
                    NewFile.WriteLine("|" + ParsedList[0] + "|" + ParsedList[1] + "|");
                    for (var i = 2; i < ParsedList.Count;)
                    {
                        if (i + NumberPerLine < ParsedList.Count)
                        {
                            // Console.WriteLine("text is {0}", ParsedList[i + 2]);

                            string FinalString = "";

                            int line = 0;
                            for (; line < NumberPerLine; line++)
                            {
                                int remplaceit = 0;
                                for (int z = 0; z < RemplaceText.Count; z++)
                                {
                                    if (RemplaceText[z] == line)
                                    {
                                        remplaceit = line;
                                        break;
                                    }
                                }

                                if (remplaceit > 0)
                                {
                                    StructTranslate2 result = TranslateStored.Find(x => x.id == ParsedList[i + 0].Replace(System.Environment.NewLine, string.Empty));

                                    if (remplaceit == RemplaceText[0])
                                    {
                                        if (result.translate1 == null)
                                        {
                                            FinalString += ParsedList[i + line] + "|";
                                        }
                                        else
                                        {
                                            FinalString += result.translate1 + "|";
                                        }
                                    }
                                    else
                                    {
                                        if (result.translate2 == null)
                                        {
                                            FinalString += ParsedList[i + line] + "|";
                                        }
                                        else
                                        {
                                            FinalString += result.translate2 + "|";
                                        }
                                    }
                                }
                                else
                                {
                                    FinalString += ParsedList[i + line] + "|";
                                }
                            }
                            NewFile.WriteLine(FinalString);

                        }

                        i = i + NumberPerLine;
                    }
                }

            }
            else if(Filename == "TextIndex.ini" || 
                Filename == "Battlefield.ini" ||
                Filename == "BeastsTower.ini" ||
                Filename == "Collection.ini"  ||
                Filename == "Dialogue.ini" ||
                Filename == "Mission.ini" ||
                Filename == "Monster.ini" ||
                Filename == "Npc.ini" ||
                Filename == "ItemCombo.ini" ||
                Filename == "PointAbility.ini" ||
                Filename == "VIP.ini") 
            { 

                //Now Store on memory translate
                int NumberPerLineTranslate = 2;
                List<StructTranslate1> TranslateStored = new List<StructTranslate1>();
                for (var i = 0; i < ParsedTranslateList.Count;)
                {

                    if (i + NumberPerLineTranslate < ParsedTranslateList.Count)
                    {
                        StructTranslate1 newTrad;
                        newTrad.id = ParsedTranslateList[i + 0].Replace(System.Environment.NewLine, string.Empty);
                        newTrad.translate1 = ParsedTranslateList[i + 1];
                        TranslateStored.Add(newTrad);
                       
                    }

                    i = i + NumberPerLineTranslate;
                }

                //Now we read original file

                if (File.Exists(filepath+".bak"))
                {
                    File.Delete(filepath + ".bak");
                }

                if (File.Exists(filepath))
                {
                    System.IO.File.Move(filepath, filepath+".bak");
                }

                //readText - Full data
                //ParsedList - List of |
                //NumberPerLine - number of while

                //And we patch it.


                using (System.IO.StreamWriter NewFile = new StreamWriter(filepath, false, Encoding.GetEncoding("big5")))//new System.IO.StreamWriter(filepath))
                {
                    List<int> RemplaceText = CreateTranslateLine(Filename);
                    NewFile.WriteLine("|" + ParsedList[0] + "|" + ParsedList[1] + "|");
                    for (var i = 2; i < ParsedList.Count;)
                    {
                        if (i + NumberPerLine < ParsedList.Count)
                        {
                            // Console.WriteLine("text is {0}", ParsedList[i + 2]);

                            string FinalString = "";

                            int line = 0;
                            for (; line < NumberPerLine; line++)
                            {
                                int remplaceit = 0;
                                for (int z = 0; z < RemplaceText.Count; z++)
                                {
                                    if (RemplaceText[z] == line)
                                    {
                                        remplaceit = line;
                                        break;
                                    }
                                }

                                if (remplaceit > 0)
                                {
                                    StructTranslate1 result = TranslateStored.Find(x => x.id == ParsedList[i + 0].Replace(System.Environment.NewLine, string.Empty));
                                    if (result.translate1 == null)
                                    {
                                        FinalString += ParsedList[i + line] + "|";
                                    }
                                    else
                                    {
                                        FinalString += result.translate1 + "|";
                                    }
                                }
                                else
                                {
                                    FinalString += ParsedList[i + line] + "|";
                                }
                            }
                            NewFile.WriteLine(FinalString);

                        }

                        i = i + NumberPerLine;
                    }
                }

            }


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
                    return new List<int>(){};
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
