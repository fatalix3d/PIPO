using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class KeyboardData {
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

        return new byte[10]
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
                0
        };
    }
}
