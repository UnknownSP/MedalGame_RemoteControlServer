using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using UnityEditor.Experimental.GraphView;
using Unity.VisualScripting;
using System;

public class TCPServer : MonoBehaviour
{

    [SerializeField] public string ipAddress = "192.168.1.16";
    [SerializeField] public int Port = 2001;

    private TcpListener tcpListener;
    private TcpClient tcpClient;
    private NetworkStream networkStream;

    private string recvMess = string.Empty;
    public bool _connected = false;
    public bool _received = false;

    private void Awake()
    {
        Task.Run( () => OnProcess() );
    }

    private void OnProcess()
    {
        var getIPAddress = IPAddress.Parse(ipAddress);
        tcpListener = new TcpListener(getIPAddress, Port);
        tcpListener.Start();

        Debug.Log("Connecting...");

        //クライアントからの接続待機
        tcpClient = tcpListener.AcceptTcpClient();
        _connected = true;
        Debug.Log("Connected");

        //クライアントからの文字列送信待機
        networkStream = tcpClient.GetStream();

        while (true)
        {
            var rcvbuffer = new byte[256];
            var count = networkStream.Read(rcvbuffer, 0, rcvbuffer.Length);

            //クライアントからの接続が切断された場合
            if (count == 0)
            {
                _connected = false;
                Debug.Log("Disconnected");

                //インスタンスを破棄
                OnDestroy();

                //再度待機
                Task.Run(() => OnProcess());

                break;
            }

            //クライアントから受信した場合
            var message = Encoding.UTF8.GetString(rcvbuffer, 0, count);
            recvMess = string.Empty;
            recvMess = message;
            _received = true;

        }
    }


    private void OnDestroy()
    {
        networkStream?.Dispose();
        tcpClient?.Dispose();
        tcpListener?.Stop();
    }

    //データ送信
    public void SendData(string str)
    {
        try
        {
            var buffer = Encoding.UTF8.GetBytes(str);
            networkStream.Write(buffer, 0, buffer.Length);
        }
        catch(Exception)
        {
            Debug.Log("Send Failed");
        }
    }

    //データ受信
    public string ReceiveData()
    {
        if (_received)
        {
            _received = false;
            return recvMess;
        }
        return "NoMessages";
    }

}
