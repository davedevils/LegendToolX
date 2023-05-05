using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Please don't read here , you will turn blind after this.
class HeadTester
{
    public static bool IsValidDDSFile(byte[] data)
    {
        if (data.Length < 4)
        {
            return false;
        }

        return data[0] == 0x44 && data[1] == 0x44 && data[2] == 0x53 && data[3] == 0x20;
    }
    public static bool IsValidIniFile(byte[] data)
    {
        string firstLine = Encoding.ASCII.GetString(data).Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[0];
        return firstLine.StartsWith("|") && firstLine.EndsWith("|");
    }

    public static bool IsValidIni2File(byte[] data)
    {
        string firstLine = Encoding.ASCII.GetString(data).Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[0];
        return firstLine.StartsWith("[") && firstLine.EndsWith("]");
    }

    public static bool IsValidIniMapFile(byte[] data)
    {
        string firstLine = Encoding.ASCII.GetString(data).Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[0];
        return firstLine.StartsWith("[") && firstLine.EndsWith("],");
    }

    public static bool IsValidLayoutFile(byte[] data)
    {
        string firstLine = Encoding.ASCII.GetString(data).Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[0];
        return firstLine.StartsWith("<?xml");
    }

    public static bool IsValidNifFile(byte[] data)
    {
        if (data.Length < 40)
        {
            return false;
        }
        string headerString = Encoding.ASCII.GetString(data, 0, Math.Min(40, data.Length));
        bool GamebryoHeader = headerString.StartsWith("Gamebryo File Format");

        if (!GamebryoHeader)
        {
            return false;
        }

        return true;
    }

    public static bool IsValidKMFFile(byte[] data)
    {
        if (data.Length < 20)
        {
            return false;
        }
        string headerString = Encoding.ASCII.GetString(data, 0, Math.Min(20, data.Length));
        bool KMFHeader = headerString.StartsWith(";Gamebryo KFM File");
        bool KMFHeader2 = headerString.StartsWith("Gamebryo KFM File");

        if (!KMFHeader && !KMFHeader2)
        {
            return false;
        }

        return true;
    }
}
