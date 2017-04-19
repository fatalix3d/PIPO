using UnityEngine;
using System.Collections;
//using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System;


public delegate void IncomingReadHandler(SocketRead read, byte[] data);
public delegate void IncomingReadErrorHandler(SocketRead read, Exception exception);
public delegate void DisconnectHandler();

public class SocketRead
{
    public const int kBufferSize = 2048;

    Socket socket;
    IncomingReadHandler readHandler;
    IncomingReadErrorHandler errorHandler;
    DisconnectHandler disconnectHandler;
    byte[] buffer = new byte[kBufferSize];

    public Socket Socket
    {
        get
        {
            return socket;
        }
    }

    //Socket read function;
    SocketRead(Socket socket, IncomingReadHandler readHandler, DisconnectHandler disconnectHandler = null, IncomingReadErrorHandler errorHandler = null)
    {
        this.socket = socket;
        this.readHandler = readHandler;
        this.errorHandler = errorHandler;
        this.disconnectHandler = disconnectHandler;

        BeginReceive();
    }

    //Begin recive data;
    void BeginReceive()
    {
        socket.BeginReceive(buffer, 0, kBufferSize, SocketFlags.None, new AsyncCallback(OnReceive), this);
    }


    public static SocketRead Begin(Socket socket, IncomingReadHandler readHandler, DisconnectHandler disconnectHandler = null, IncomingReadErrorHandler errorHandler = null)
    {
        return new SocketRead(socket, readHandler, disconnectHandler, errorHandler);
    }


    void OnReceive(IAsyncResult result)
    {
        try
        {
            if (result.IsCompleted)
            {
                if (socket.Connected)
                {
                    int bytesRead = socket.EndReceive(result);
                    if (bytesRead > 0)
                    {
                        byte[] read = new byte[bytesRead];
                        Array.Copy(buffer, 0, read, 0, bytesRead);

                        readHandler(this, read);
                        Begin(socket, readHandler, disconnectHandler, errorHandler);
                    }
                }
                else if (disconnectHandler != null)
                {
                    disconnectHandler();
                }
            }
        }
        catch (Exception e)
        {
            if (errorHandler != null)
            {
                errorHandler(this, e);
            }
        }
    }
}
