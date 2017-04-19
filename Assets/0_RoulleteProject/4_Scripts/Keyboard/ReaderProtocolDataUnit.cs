using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
public class ReaderProtocolDataUnit
{
    public Byte RS232SyncLabel
    {
        get;
        set;
    }
    public Int16 Length
    {
        get;
        set;
    }
    public Byte Command
    {
        get;
        set;
    }
    public Byte SequenceId
    {
        get;
        set;
    }
    public Byte Destination
    {
        get;
        set;
    }
    public Byte Source
    {
        get;
        set;
    }
    public Byte SourceType
    {
        get;
        set;
    }

    public virtual byte[] Serialize()
    {
        Length -= 4;

        return new byte[256]
        {
                this.RS232SyncLabel,
                (byte)(this.Length % 256),
                (byte)(this.Length / 256),
                this.Command,
                this.SequenceId,
                this.Destination,
                this.Source,
                this.SourceType,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0

        };
    }

    public virtual void Deserialize(byte[] Data)
    {
        if ((Data == null) || (Data.Length < 8))
            throw new SerializationException();
        this.RS232SyncLabel = Data[0];
        this.Length = (Int16)((Data[2] << 8) | Data[1]);
        this.Command = Data[3];
        this.SequenceId = Data[4];
        this.Destination = Data[5];
        this.Source = Data[6];
        this.SourceType = Data[7];
    }
    protected byte PayloadLength
    {
        get;
        set;
    }
    protected byte[] TransformBytes(byte[] data, byte offset, byte length)
    {
        byte i = 0;
        List<byte> p = new List<byte>(128);
        do
        {
            var b = data[offset + i];
            switch (b)
            {
                case 0x52:
                    {
                        byte d = (byte)(data[offset + i + 1] ^ b);
                        p.Add(d);
                        i += 2;
                    }
                    break;
                default:
                    {
                        p.Add(b);
                        i++;
                    }
                    break;
            }
        } while (i < length);
        return p.ToArray();
    }

    protected byte getPayloadLength(byte[] bytes, byte offset)
    {
        return (byte)((bytes.Length - 1) - offset);
    }

    protected UInt64 NumberFromBytes(byte[] bytes, byte offset)
    {
        byte[] payload = TransformBytes(bytes, offset, this.PayloadLength);
        UInt64 number = 0x00;
        number = (UInt64)(
               (((ulong)payload[7]) << 56) |
               (((ulong)payload[6]) << 48) |
               (((ulong)payload[5]) << 40) |
               (((ulong)payload[4]) << 32) |
               (((ulong)payload[3]) << 24) |
               (((ulong)payload[2]) << 16) |
               (((ulong)payload[1]) << 8) |
                ((ulong)payload[0]));
        return number;

    }

    internal static ulong NumberFromBytes(byte[] newBytes, int v)
    {
        throw new NotImplementedException();
    }
}
