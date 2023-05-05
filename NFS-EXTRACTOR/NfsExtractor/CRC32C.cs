using System;

public class CRC32C
{
    private static readonly uint[] crc32CTable = new uint[256];

    static CRC32C()
    {
        for (uint i = 0; i < 256; i++)
        {
            uint value = i;
            for (uint j = 0; j < 8; j++)
            {
                value = (value >> 1) ^ (0x82F63B78 * Convert.ToUInt32((value & 1) != 0));
            }
            crc32CTable[i] = value;
        }
    }

    public static uint CalculateCRC32C(uint[] data, uint dataSize, uint initialValue = 0xFFFFFFFF)
    {
        uint crc = initialValue;

        for (uint i = 0; i < dataSize; i++)
        {
            crc = crc32CTable[(crc ^ data[i]) & 0xFF] ^ (crc >> 8);
        }

        return ~crc;
    }
}
