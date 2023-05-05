using System;
using System.Collections.Generic;
using System.IO;

public class FileEntry
{
    public ulong Hash { get; set; }
    public string NFSFile { get; set; }

    public uint Time { get; set; }
    public uint CompressedSize { get; set; }
    public uint UncompressedSize { get; set; }
    public uint CheckSumCompress { get; set; }
    public uint CheckSumUncompress { get; set; }

    // ADD unrelated to real file
    public string NFSPath { get; set; }
}

public class FileListPCParser
{
    public static List<FileEntry> Parse(string filePath)
    {
        var fileEntries = new List<FileEntry>();

        using (var reader = new StreamReader(filePath))
        {
            // Read and discard the first line
            reader.ReadLine();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] parts = line.Split(',');

                if (parts.Length < 6)
                    continue;

                string ChunkHash = parts[0];
                string file = parts[1];
                string Time = parts[2];
                string CompressSize = parts[3];
                string UncompressSize = parts[4];
                string CheckSumCompress = parts[5];
                string CheckSumUncompress = parts[6];


                if (file.Contains("/"))
                    continue; // skip normal file

                string folderName = file.Substring(0, 1);

                fileEntries.Add(new FileEntry
                {
                    // Updated hash to 64 byte cause long now
                    Hash = Convert.ToUInt64(ChunkHash, 16),
                    NFSFile = file,
                    Time = Convert.ToUInt32(Time, 16),
                    CompressedSize = Convert.ToUInt32(CompressSize),
                    UncompressedSize = Convert.ToUInt32(UncompressSize),
                    CheckSumCompress = Convert.ToUInt32(CheckSumCompress,16),
                    CheckSumUncompress = Convert.ToUInt32(CheckSumUncompress, 16),
                    NFSPath = folderName+ "\\",
                });
            }
        }

        return fileEntries;
    }
}
