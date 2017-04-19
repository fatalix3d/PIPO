using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System;


public delegate void OnClientConnected(Socket socket);
public delegate void OnError(string message);

public class Client
{
    private Socket socket;

    //public event OnClientStarted OnClientStarted = (s) => {};
    public event OnClientConnected OnClientConnected = (s) => { };

    public event OnError OnError = OnErrorDefault;

    private static void OnErrorDefault(string message)
    {
        //Debug.LogError( message );
    }

    public bool Connect(string host, int port)
    {
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(host, port);
        }
        catch (System.Exception e)
        {
            socket.Close();
            socket = null;
            OnError("Failed to connect to " + host + " on port " + port);
            return false;
        }
        return true;
    }

    public bool IsConnected { get { return socket != null && socket.Connected; } }

    public void Disconnect()
    {
        Debug.Log( "Disconnect" );
        if (IsConnected)
        {
            socket.BeginDisconnect(false, new AsyncCallback(OnEndHostComplete), socket);
        }
    }

    private void OnClientConnect(System.IAsyncResult result)
    {
        try
        {
            OnClientConnected(socket.EndAccept(result));
        }
        catch (System.Exception e)
        {
            OnError("Exception when accepting incoming connection: " + e);
        }

        try
        {
            socket.BeginAccept(new AsyncCallback(OnClientConnect), socket);
        }
        catch (Exception e)
        {
            OnError("Exception when starting new accept process: " + e);
        }
    }

    private void OnEndHostComplete(System.IAsyncResult result)
    {
        socket = null;
    }

    public bool Send(byte[] bytes)
    {
        int sendReturn = 0;

        try
        {
            //Debug.Log( "Sending -> " + BitConverter.ToString( bytes ) );
            sendReturn = socket.Send(bytes);
            //Debug.Log( "Sended -> " + BitConverter.ToString( bytes ) + " -> " + sendReturn );
        }
        catch (Exception e)
        {
            OnError("Sending exception: " + e);
        }
        return sendReturn > 0;
    }

    internal SocketRead BeginReceive(IncomingReadHandler readHandler, DisconnectHandler disconnectHandler = null, IncomingReadErrorHandler errorHandler = null)
    {
        //Debug.Log( "BeginReceive" + socket );
        return SocketRead.Begin(socket, readHandler, disconnectHandler, errorHandler);
    }
}
