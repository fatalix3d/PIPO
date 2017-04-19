using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;


public class SerialPortReader
{
    static private bool pktBeginReceived = false;
    static private int bytesRead = 0;
    static private int replacementCharValue = 0x00;
    static private List<byte> buffer = new List<byte>();
    static object locker = new object();
    public static String GetAnswer()
    {
        return String.Empty;
    }
    public static String ReadPortData(SerialPort Port, int pktLength)
    {

        lock (locker)
        {
            do
            {
                var b = (byte)Port.ReadByte();
                switch (b)
                {
                    case 0x00:
                        break;
                    case 0xaa:
                        if (!pktBeginReceived)
                        {
                            pktBeginReceived = true;
                            buffer.Add(b);
                            bytesRead++;
                        }
                        break;
                    case 0x52:
                        if (pktBeginReceived)
                            replacementCharValue = b;
                        break;
                    default:
                        {
                            if (pktBeginReceived)
                            {
                                if (replacementCharValue != 0x00)
                                {
                                    buffer.Add((byte)(replacementCharValue ^ b));
                                    replacementCharValue = 0x00;
                                }
                                else
                                    buffer.Add(b);
                                bytesRead++;
                            }
                        }
                        break;
                }

            } while (bytesRead < pktLength);
            if (pktBeginReceived)
            {
                bytesRead = 0;
                pktBeginReceived = false;
                replacementCharValue = 0x00;
                var data = Convert.ToBase64String(buffer.Take(pktLength).ToArray());
                buffer.Clear();
                Port.DiscardInBuffer();
                return data;
            }
            else
                return String.Empty;
        }
    }
}
