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


            Console.WriteLine("--------------------------");
            Console.WriteLine("| Translate File Patcher |");
            Console.WriteLine("--------------------------");

            string dbPath = @"db";
            string outPath = @"out";

            foreach (var prefix in new[] { "c_", "s_" })
            {
                foreach (var filePath in Directory.GetFiles(dbPath, $"{prefix}*"))
                {
                    string fileNameWithoutPrefix = Path.GetFileName(filePath).Substring(2);
                    string tFilePath = Path.Combine(outPath, $"t_{fileNameWithoutPrefix}");

                    if (File.Exists(tFilePath))
                    {
                        var translations = ReadTranslateFile(tFilePath);
                        PatchTranslate(filePath, translations);

                        Console.WriteLine($"{fileNameWithoutPrefix} patched file with new translate.");
                    }
                    else
                    {
                        Console.WriteLine($"t_{fileNameWithoutPrefix} file don't exist.");
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

        public static int GetNumberOfCol(string line)
        {
            if (line.StartsWith("|V."))
            {
                // official reading
                string[] parts = line.Split('|');
                if (parts.Length > 2)
                {
                    return int.Parse(parts[2]);
                }
            }
            else
            {
                // dumbfile like dave
                return int.Parse(line.Split('|')[0]);
            }

            return 0;
        }


        public static Dictionary<string, (int[] TargetColumns, string[] Translations)> ReadTranslateFile(string tFilePath)
        {
            var translations = new Dictionary<string, (int[] TargetColumns, string[] Translations)>();
            Encoding encoding = GetEncodingFile(tFilePath);

            using (StreamReader reader = new StreamReader(tFilePath, encoding))
            {
                string[] lines = reader.ReadToEnd().Split(new[] { Environment.NewLine }, StringSplitOptions.None);

                int[] targetColumns = lines[1].Split('|').Where(s => !string.IsNullOrEmpty(s)).Select(int.Parse).ToArray();

                for (int i = 2; i < lines.Length; i++) 
                {
                    string[] parts = lines[i].Split('|');
                    if (parts.Length > 1)
                    {
                        translations[parts[0]] = (targetColumns, parts.Skip(1).ToArray());
                    }
                }
            }

            return translations;
        }

        public static void PatchTranslate(string cFilePath, Dictionary<string, (int[] TargetColumns, string[] Translations)> translationsData)
        {
            Encoding encoding = GetEncodingFile(cFilePath);
            var lines = File.ReadAllLines(cFilePath, encoding).ToList();

            for (int i = 0; i < lines.Count; i++)
            {
                string[] parts = lines[i].Split('|');
                if (translationsData.ContainsKey(parts[0]))
                {
                    var data = translationsData[parts[0]];
                    for (int j = 0; j < data.TargetColumns.Length; j++)
                    {
                        int targetColumn = data.TargetColumns[j];
                        if (targetColumn < parts.Length && j < data.Translations.Length)
                        {
                            parts[targetColumn] = data.Translations[j];
                        }
                    }

                    lines[i] = string.Join("|", parts);
                }
            }

            File.WriteAllLines(cFilePath, lines, encoding);
        }

        static System.Text.Encoding GetEncodingFile(string filePath)
        {
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                byte[] bom = new byte[4];
                file.Read(bom, 0, 4);
                if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) // utf-8 
                {
                    return Encoding.UTF8;
                }
                else if (bom[0] == 0xff && bom[1] == 0xfe || bom[0] == 0xfe && bom[1] == 0xff) // Unicode
                {
                    return Encoding.Unicode;
                }
                else
                {
                    return Encoding.GetEncoding("big5");
                }
            }
        }

    }
}
