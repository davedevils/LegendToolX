using Force.Crc32;
using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;


public class Chunk
{
    public ulong Hash { get; set; }
    public uint Offset { get; set; }
    public uint Size { get; set; }
    public uint Checksum { get; set; }
    public uint Time { get; set; }

}

class NFSManager
{
    public Dictionary<ulong, Chunk> _chunks;
    private string _filePath;
    public bool valid = false;

    public NFSManager()
    {

    }
    public NFSManager(string filePath, int xor = 0)
    {
        // Load the packageindex OR 
        _chunks = new Dictionary<ulong, Chunk>();
        _filePath = filePath;

        using (var fileStream = File.OpenRead(filePath))
        {
            using (var binaryReader = new BinaryReader(fileStream))
            {
                uint version = binaryReader.ReadUInt32();


                if(version != 0x20151018 && version != 0x20190503)
                {
                    //WUTT ?? what this ?
                    string boxmsg = "This version is not supported ! Can be buggy or worst !";
                    string boxtitle = "WARNING";
                    MessageBox.Show(boxmsg, boxtitle);
                }

                while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
                {
                   
                    ulong hash;
                    if (version == 0x20190503)
                    {
                        //0x20190503 - ulong? Really ?
                        hash = binaryReader.ReadUInt64();
                    }
                    else
                    {
                        hash = binaryReader.ReadUInt32();
                    }


                    

                    Chunk chunk = new Chunk
                    {
                        Offset = binaryReader.ReadUInt32(),
                        Size = binaryReader.ReadUInt32(),
                        Checksum = binaryReader.ReadUInt32(),
                        Time = binaryReader.ReadUInt32(),

                        // cause i'm lazy
                        Hash = hash,
                    };

                    if (xor == 1)
                    {
                        chunk.Offset = chunk.Offset ^ (uint)hash;
                        chunk.Size = chunk.Size ^ (uint)hash;
                    }

                    _chunks.Add(hash, chunk);

                }
                valid = true;
            }
        }
    }

    public void ExtractChunkToFile(string sFileName, ulong nHash, string OutFile)
    {
        if (!_chunks.ContainsKey(nHash))
        {
            Console.WriteLine("Chunk not found!");
            return;
        } else {
            Chunk chunk = _chunks[nHash];

            using (var fileStream = File.OpenRead(sFileName))
            {
                using (var binaryReader = new BinaryReader(fileStream))
                {
                    fileStream.Seek(chunk.Offset, SeekOrigin.Begin);
                    byte[] data = binaryReader.ReadBytes(Convert.ToInt32(chunk.Size));

                    if (OutFile != "")
                    {
                        File.WriteAllBytes(OutFile, data);
                    }
                }
            }
        }
    }

    public void ExtractChunkToFile(string sFileName, ulong nHash, uint Offset, uint Size, string OutFile)
    {
        if (!_chunks.ContainsKey(nHash))
        {
            Console.WriteLine("Chunk not found!");
            return;
        }
        else
        {
            Chunk chunk = _chunks[nHash];

            using (var fileStream = File.OpenRead(sFileName))
            {
                using (var binaryReader = new BinaryReader(fileStream))
                {
                    // Feed me DEAD BEEF PLZ
                    var byted = new byte[] { 0xDE, 0xAD, 0xBE, 0xEF };
                    string extentionfinder = Path.GetExtension(OutFile);
                    fileStream.Seek(Offset, SeekOrigin.Begin);
                    byte[] data = binaryReader.ReadBytes(Convert.ToInt32(Size) + 0x0C);
                    byte[] decompressedData = DecompressUsingZlibStream(data, 0x0C);
                    if (decompressedData.SequenceEqual(byted))
                    {
                       fileStream.Seek(Offset, SeekOrigin.Begin);
                       data = binaryReader.ReadBytes(Convert.ToInt32(Size) + 0x10);
                       decompressedData = DecompressUsingZlibStream(data, 0x10);
                    }

                    if (extentionfinder == "")
                    {
                        // Now try to find extention
                        if (HeadTester.IsValidDDSFile(decompressedData))
                            extentionfinder = ".dds";
                        else if (HeadTester.IsValidNifFile(decompressedData))
                            extentionfinder = ".nif";
                        else if (HeadTester.IsValidKMFFile(decompressedData))
                            extentionfinder = ".kmf";
                        else if (HeadTester.IsValidIniFile(decompressedData))
                            extentionfinder = ".ini";
                        else if (HeadTester.IsValidIni2File(decompressedData))
                            extentionfinder = ".ini";
                        else if (HeadTester.IsValidIniMapFile(decompressedData))
                            extentionfinder = ".ini";
                        else if (HeadTester.IsValidLayoutFile(decompressedData))
                            extentionfinder = ".layout";
                    }
                    else
                    {
                        // no need add 2x ext
                        extentionfinder = "";
                    }

                    File.WriteAllBytes(OutFile + extentionfinder, decompressedData);
                }
            }
        }
    }

    static byte[] DecompressUsingZlibStream(byte[] inputData, int offset = 0x10)
    {
        try
        {
            // Skip the first byte
            byte[] compressedData = new byte[inputData.Length - offset];
            Buffer.BlockCopy(inputData, offset, compressedData, 0, compressedData.Length);

            using (MemoryStream inputStream = new MemoryStream(compressedData))
            using (ZlibStream zlibStream = new ZlibStream(inputStream, CompressionMode.Decompress))
            using (MemoryStream outputStream = new MemoryStream())
            {
                zlibStream.CopyTo(outputStream);
                return outputStream.ToArray();
            }
        }
        catch (IOException)
        {
            // DEAD BEEF
            var byted = new byte[] { 0xDE, 0xAD, 0xBE, 0xEF };
            return byted;
        }
        catch (ZlibException)
        {
            var byted = new byte[] { 0xDE, 0xAD, 0xBE, 0xEF };
            return byted;
        }
    }

    public string FindFileForChunk(string basePath, Chunk chunk)
    {

        foreach (string filePath in Directory.EnumerateFiles(basePath, "*.*", SearchOption.AllDirectories))
        {
            using (var fileStream = File.OpenRead(filePath))
            {
                using (var binaryReader = new BinaryReader(fileStream))
                {
                    if (fileStream.Length < chunk.Offset + chunk.Size)
                    {
                        continue; // File is too small to contain the chunk
                    }

                    fileStream.Seek(chunk.Offset, SeekOrigin.Begin);
                    byte[] data = binaryReader.ReadBytes(Convert.ToInt32(chunk.Size));

                    //int checksum = CRC32.Compute(data);
                    uint checksum = Crc32Algorithm.Compute(data);

                    if (checksum == chunk.Checksum)  //if (checksum == unchecked((int)chunk.Checksum))
                    {
                        return filePath; // Found the correct file
                    }
                }
            }
        }

        return null; // No file found containing the chunk
    }

    public void ExtractFile(string FileName, string FilePath, string destinationPath)
    {
        Chunk chunk = GetFile(FileName, FilePath);

        if (chunk == null)
        {
            // AMMAA SAD
            throw new FileNotFoundException($"File not found in the package: {FileName}");
        }

        (uint packageName, uint fileHash) = HashName(FileName, FilePath);

        using (var packageStream = new FileStream(FilePath+"//"+ FileName, FileMode.Open, FileAccess.Read))
        using (var packageReader = new BinaryReader(packageStream))
        using (var destinationStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write))
        using (var destinationWriter = new BinaryWriter(destinationStream))
        {
            packageReader.BaseStream.Seek(chunk.Offset, SeekOrigin.Begin);

            byte[] buffer = new byte[chunk.Size];
            packageReader.Read(buffer, 0, (int)chunk.Size);
            destinationWriter.Write(buffer);
        }
    }

    public void ExtractAllFiles(string outputDirectory)
    {
        foreach (var fileHash in _chunks.Keys)
        {
            Chunk chunk = _chunks[fileHash];
            string fileName = $"file_{fileHash}.bin";
            string destinationPath = Path.Combine(outputDirectory, fileName);

            ExtractFile(fileName, string.Empty, destinationPath);
        }
    }

    // PART for BULK NAME FINDER - Scratter Generator

    public Chunk GetFile(string fileName, string filePath)
    {
        (uint packageName, uint fileHash) = HashName(fileName, filePath);

        if (!_chunks.TryGetValue(fileHash, out Chunk chunk))
        {
            return null;
        }

        return chunk;
    }

    private const int PackageNameCount = 4;
    private (uint packageName, uint fileName) HashName(string name, string path)
    {
        uint packageName = 0;
        uint fileName = 0;

        for (int i = 0; i < Math.Min(name.Length, PackageNameCount); i++)
        {
            packageName = Hash(name[i], packageName);
        }

        foreach (char c in name + path)
        {
            fileName = Hash(c, fileName);
        }

        return (packageName, fileName);
    }

    private uint Hash(char c, uint seed)
    {
        // MAGIC TRIKS - WELCOME TO THE CLUB NOW
        return (seed * 0x1000193) ^ c;
    }
}
