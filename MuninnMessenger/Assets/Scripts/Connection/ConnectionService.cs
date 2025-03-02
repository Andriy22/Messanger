using System;
using System.Collections;
using NativeWebSocket;
using UnityEngine;

public class ConnectionService : MonoBehaviour
{
    private static ConnectionService _instance;

    private WebSocket _socket;
    private long _loginToken;
    private User _user;

    public static User User => _instance._user;

    private void Awake()
    {
        _user = Mocks.USERS[0];

        _instance = this;
    }

    public IEnumerator Load()
    {
        _socket = new WebSocket(Consts.SERVER_URL);

        _socket.OnOpen += OnOpen;
        _socket.OnError += OnError;
        _socket.OnClose += OnClose;
        _socket.OnMessage += OnMessage;

        yield return _socket.Connect();
    }

    private void OnMessage(byte[] data)
    {
        Debug.Log("OnMessage!");

        var message = System.Text.Encoding.UTF8.GetString(data);
        Debug.Log(message);
    }

    private void OnClose(WebSocketCloseCode closeCode)
    {
        Debug.Log("Connection closed!");
    }

    private static void OnError(string e)
    {
        Debug.Log("Error! " + e);
    }

    private void OnOpen()
    {
        Debug.Log("Connection open!");
    }
}