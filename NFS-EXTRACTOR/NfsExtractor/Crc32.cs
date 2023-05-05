using System;
using System.IO;


internal class CRC32
{
    private const uint Polynomial = 0x82F63B78;
    private const uint Initial = uint.MaxValue;
    private const uint CRCNumTables = 8;
    private static readonly uint[] Table;

    private uint _value;

    public int Value => (int)(~_value);

    static CRC32()
    {
        Table = new uint[2048];
        uint num;
        for (num = 0; num < 256; num++)
        {
            uint num2 = num;
            for (int i = 0; i < 8; i++)
            {
                num2 = ((num2 >> 1) ^ (uint)(-2097792136 & (int)(~((num2 & 1) - 1))));
            }
            Table[num] = num2;
        }
        for (; num < 2048; num++)
        {
            uint num3 = Table[num - 256];
            Table[num] = (Table[num3 & 0xFF] ^ (num3 >> 8));
        }
    }

    public CRC32()
    {
        Init();
    }

    public void Init()
    {
        _value = uint.MaxValue;
    }

    public void UpdateByte(byte b)
    {
        _value = ((_value >> 8) ^ Table[(byte)_value ^ b]);
    }

    public void Update(byte[] buffer, int offset, int count)
    {
        if (buffer == null) throw new ArgumentNullException(nameof(buffer));
        if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), "Offset must be positive.");
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), "Count must be positive.");
        if (buffer.Length - offset < count) throw new ArgumentException("Offset + Count is greater than buffer.Length.");

        if (count == 0) return;

        uint[] table = Table;
        uint num = _value;
        while ((offset & 7) != 0 && count != 0)
        {
            num = ((num >> 8) ^ table[(byte)num ^ buffer[offset++]]);
            count--;
        }

        if (count >= 8)
        {
            int num2 = (count - 8) & -8;
            count -= num2;
            num2 += offset;
            while (offset != num2)
            {
                num ^= (uint)(buffer[offset] + (buffer[offset + 1] << 8) + (buffer[offset + 2] << 16) + (buffer[offset + 3] << 24));
                uint num3 = (uint)(buffer[offset + 4] + (buffer[offset + 5] << 8) + (buffer[offset + 6] << 16) + (buffer[offset + 7] << 24));
                offset += 8;
                num = (table[(byte)num + 1792] ^ table[(byte)(num >>= 8) + 1536] ^ table[(byte)(num >>= 8) + 1280] ^ table[(num >> 8) + 1024] ^ table[(byte)num3 + 768] ^ table[(byte)(num3 >>= 8) + 512] ^ table[(byte)(num3 >>= 8) + 256] ^ table[num3 >> 8]);
            }
        }
        while (count-- != 0)
        {
            num = ((num >> 8) ^ table[(byte)num ^ buffer[offset++]]);
        }
        _value = num;
    }

    internal static int Compute(Stream input)
    {
        if (input == null) throw new ArgumentNullException(nameof(input));

        CRC32 crc = new CRC32();
        byte[] buffer = new byte[16384];
        int count;
        while ((count = input.Read(buffer, 0, buffer.Length)) > 0)
        {
            crc.Update(buffer, 0, count);
        }
        return crc.Value;
    }

    internal static int Compute(string input)
    {
        if (input == null) throw new ArgumentNullException(nameof(input));
        if (!File.Exists(input)) throw new FileNotFoundException($"The specified file does not exist: {input}");

        using (FileStream inputStream = File.OpenRead(input))
        {
            return Compute(inputStream);
        }
    }

    internal static int Compute(byte[] buffer, int offset, int count)
    {
        if (buffer == null) throw new ArgumentNullException(nameof(buffer));
        if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), "Offset must be positive.");
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), "Count must be positive.");

        CRC32 crc = new CRC32();
        crc.Update(buffer, offset, count);
        return crc.Value;
    }

    internal static int Compute(byte[] buffer)
    {
        if (buffer == null) throw new ArgumentNullException(nameof(buffer));

        return Compute(buffer, 0, buffer.Length);
    }
}
