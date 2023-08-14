using System;
using System.IO;
using System.IO.Compression;
using Ionic.Zip;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;

public class HolyBeastUnzipper
{
    private static readonly int[] PasswordArray = new int[]
    {
        0x59, 0x182, 0x313, 0x39E, 0x308, 0x149, 0x210, 0x206, 0x96, 0x178
    };
    private static Dictionary<string, string> folder = new Dictionary<string, string>
    {
        { "effect", "effect" },
        { "map", "map" },
        { "data", "data" },
        { "char", "char" },
        { "item", "item" },
        { "sys", "sys" },
        { "ui", "UI" },
        { "uimap", "UI\\Map" },
        { "uiicon", "UI\\Icon" },
        { "effectanimation", "effect\\animation" },
        { "effectmodel", "effect\\model" },
        { "effecttexture", "effect\\texture" },
        { "itemanimation", "item\\animation" },
        { "itemmodel", "item\\model" },
        { "itemmodeltexture", "item\\model\\texture" },
        { "itemtexture", "item\\texture" },
        { "mapanimation", "map\\animation" },
        { "mapmodel", "map\\model" },
        { "mapmodelsky", "map\\model\\sky" },
        { "mapmodelobject", "map\\model\\object" },
        { "maptexture", "map\\texture" },
        { "charanimation", "char\\animation" },
        { "charmodel", "char\\model" },
        { "chartexture", "char\\texture" },
        { "datadb", "Data\\db" },
        { "sysmodel", "sys\\model" },
        { "systexture", "sys\\texture" },
    };

    public static void Main(string[] args)
    {
        Console.WriteLine("-----------------------------------------------------");
        Console.WriteLine("               HBO UNPACKER BY DAVEDEVIL'S      ");
        Console.WriteLine("-----------------------------------------------------");
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: HBOUnpacker.exe <inputDirectory> <outputDirectory>");
            Console.WriteLine("\nPress key for close this app ...");
            Console.ReadKey();
            return;
        }

        try
        {
            string inputDirectory = args[0];
            string outputDirectory = args[1];

            Dictionary<string, string> mappingfolder = new Dictionary<string, string>(folder);
            for (int i = 0; i <= 5; i++)
            {
                for (int z = 0; z < 36; z++)
                {
                    for (int j = 1; j <= 99; j++)
                    {
                        if (z < 10)
                        {
                            mappingfolder["mapmodel" + i + z + "_" + j.ToString("00")] = "map\\model\\" + i + z + "_" + j.ToString("00");
                            mappingfolder["mapmodel" + i + z + "_" + j.ToString("00") + "_0000"] = "map\\model\\" + i + z + "_" + j.ToString("00") + "_0000";
                            mappingfolder["maptexture" + i + z + "_" + j.ToString("00")] = "map\\texture\\" + i + z + "_" + j.ToString("00");
                        }
                        else
                        {
                            mappingfolder["mapmodel" + i + (char)('a' + z - 10) + "_" + j.ToString("00")] = "map\\model\\" + i + (char)('a' + z - 10) + "_" + j.ToString("00");
                            mappingfolder["mapmodel" + i + (char)('a' + z - 10) + "_" + j.ToString("00") + "_0000"] = "map\\model\\" + i + (char)('a' + z - 10) + "_" + j.ToString("00") + "_0000";
                            mappingfolder["maptexture" + i + (char)('a' + z - 10) + "_" + j.ToString("00")] = "map\\texture\\" + i + (char)('a' + z - 10) + "_" + j.ToString("00");
                        }
                    }
                }
            }

            foreach (var file in Directory.GetFiles(inputDirectory))
            {
                ProcessFile(file, outputDirectory, mappingfolder);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private static void ProcessFile(string inputFile, string outputDirectory, Dictionary<string, string> mappingfolder)
    {
        if (!File.Exists(inputFile))
        {
            Console.WriteLine($"The input file {inputFile} does not exist...");
            return;
        }

        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        using (FileStream fs = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
        using (ZipFile zip = ZipFile.Read(fs))
        {
            foreach (ZipEntry entry in zip)
            {
                MemoryStream decryptedStream = new MemoryStream();
                string password = GeneratePassword(entry.FileName);

                try
                {
                    // Decrypt and extract the content of the file
                    entry.ExtractWithPassword(decryptedStream, password);
                    decryptedStream.Position = 0;
                    string sanitizedFileName = SanitizeFileName(entry.FileName);
                    string NewNamePath = DecideFolderForFile(sanitizedFileName, mappingfolder);
                    string outputPath = Path.Combine(outputDirectory, NewNamePath);
                    EnsureDirectoryExists(outputPath);
                    using (FileStream outFile = new FileStream(outputPath, FileMode.Create))
                    {
                        decryptedStream.CopyTo(outFile);
                    }
                    Console.WriteLine($"Extracted: {entry.FileName} at {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to extract {entry.FileName}. Error: {ex.Message}");
                }
            }
        }
        Console.WriteLine($"Extract of {inputFile} finished !");
    }

    private static string DecideFolderForFile(string filename, Dictionary<string, string> mapfolder)
    {
        Dictionary<string, string> folderMappings;
        if (filename.StartsWith("map"))
        {
            folderMappings = new Dictionary<string, string>(mapfolder);
        }
        else
        {
            folderMappings = new Dictionary<string, string>(folder);
        }

        foreach (string prefix in folderMappings.Keys.OrderByDescending(k => k.Length).ToList())
        {
            if (filename.StartsWith(prefix) && (!filename.StartsWith(prefix + "_")))
            {
                string modifiedPath = Path.Combine(folderMappings[prefix], filename.Substring(prefix.Length));
                return modifiedPath;
            }
        }

        return filename;
    }



    private static string GeneratePassword(string name)
    {
        int nameLength = name.Length;
        int FinalByte = 0;

        char[] password = new char[nameLength];

        for (int x = 0; x < nameLength; x++)
        {
            int XOR = x % 10;
            int SwapedByte = PasswordArray[XOR];
            FinalByte = (FinalByte + SwapedByte) % nameLength;
            password[x] = name[FinalByte];
        }

        return new string(password);
    }

    // Common function used
    private static string SanitizeFileName(string fileName)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        foreach (var invalidChar in invalidChars)
        {
            fileName = fileName.Replace(invalidChar.ToString(), "");
        }
        return fileName;
    }
    private static void EnsureDirectoryExists(string fullPath)
    {
        string directoryPath = Path.GetDirectoryName(fullPath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }

}
