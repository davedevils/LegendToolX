using System;

public class ZipCrypto
{
    private uint[] keys = new uint[3];

    private static uint DecryptByte(uint[] keys)
    {
        uint temp = ((keys[2] & 0xFFFF) | 2);
        return (temp * (temp ^ 1)) >> 8;
    }

    private static void UpdateKeys(byte byteValue, uint[] keys)
    {
        keys[0] = Crc32.ComputeCrc32(keys[0], byteValue);
        keys[1] += (byte)keys[0];
        keys[1] = keys[1] * 134775813 + 1;
        keys[2] = Crc32.ComputeCrc32(keys[2], (byte)(keys[1] >> 24));
    }

    public void Initialize(string password)
    {
        keys[0] = 0x12345678;
        keys[1] = 0x23456789;
        keys[2] = 0x34567890;

        foreach (char passChar in password)
        {
            UpdateKeys((byte)passChar, keys);
        }
    }

    public byte[] Decrypt(byte[] cipherText)
    {
        byte[] plainText = new byte[cipherText.Length];
        for (int i = 0; i < cipherText.Length; i++)
        {
            byte decryptedByte = (byte)(cipherText[i] ^ DecryptByte(keys));
            UpdateKeys(decryptedByte, keys);
            plainText[i] = decryptedByte;
        }
        return plainText;
    }
}

public static class Crc32
{
    private static readonly uint[] CrcTable;

    static Crc32()
    {
        const uint polynomial = 0xedb88320;
        CrcTable = new uint[256];

        for (int i = 0; i < 256; i++)
        {
            uint crc = (uint)i;
            for (uint j = 8; j > 0; j--)
            {
                if ((crc & 1) == 1)
                    crc = (crc >> 1) ^ polynomial;
                else
                    crc >>= 1;
            }
            CrcTable[i] = crc;
        }
    }

    public static uint ComputeCrc32(uint oldCrc, byte value)
    {
        return CrcTable[(oldCrc ^ value) & 0xFF] ^ (oldCrc >> 8);
    }
}
