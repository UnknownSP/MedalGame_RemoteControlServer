using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TCPコントロール処理
public class TCPControl : MonoBehaviour
{
    [SerializeField] public TCPServer TCPServer;
    [SerializeField] public MainProcess mainProcess;
    // Start is called before the first frame update
    string[] receiveTCP = new string[] {};
    void Start()
    {
        
    }

    void Update()
    {
        //クライアントと接続していない場合は処理しない
        if (!TCPServer._connected) return;

        //TCPで受信
        receiveTCP = TCPServer.ReceiveData().Split(new string[] { ":" }, System.StringSplitOptions.None);
        //メッセージの中にテストモード終了が入っていた場合
        for (int i = 0; i < receiveTCP.Length; i++)
        {
            if (receiveTCP[i].Contains("st1_ResetTestMode"))
            {
                for (int j = 0; j < 8; j++)
                {
                    //シリアル通信で送るデータをリセット
                    mainProcess.serialSendData[j] = 0;
                }
                Debug.Log("ResetTestMode");
                return;
            }
        }

        //TCP受信サイズでループ
        for(int i=0; i<receiveTCP.Length; i++)
        {
            int receiveValue = 0;
            if (receiveTCP[i].Contains("GET"))
            {
                if (receiveTCP[i+2] != null)
                {
                    receiveValue = int.Parse(receiveTCP[i+2]);
                }
                GetHandler(receiveTCP[i+1], receiveValue);
            }
            if (receiveTCP[i].Contains("SET"))
            {
                if (receiveTCP[i+2] != null)
                {
                    receiveValue = int.Parse(receiveTCP[i+2]);
                }
                SetHandler(receiveTCP[i+1], receiveValue);
            }
        }

        //クライアントから発射処理が来た場合、進行状況によってリセットをかける
        if (mainProcess.serialSendData[0] == 100 && mainProcess.st1_processState != 0)
        {
            mainProcess.serialSendData[0] = 0;
        }
    }

    void GetHandler(string message, int value)
    {
        if (message.Contains("st1_processState"))
        {
            TCPServer.SendData("st1_processState:" + mainProcess.st1_processState + ":");
        }
    }

    void SetHandler(string message, int value)
    {
        if (message.Contains("st1_ballLaunch"))
        {
            TCPServer.SendData("st1_processState:" + mainProcess.st1_processState + ":");
            mainProcess.serialSendData[0] = 100; //100は暫定の設定値
        }
        if (message.Contains("st1_1stLED"))
        {
            mainProcess.serialSendData[1] = 131; //131,132,133はLEDテストの暫定の設定値
            mainProcess.serialSendData[2] = (byte)value;
        }
        if (message.Contains("st1_2ndLED"))
        {
            mainProcess.serialSendData[3] = 132;
            mainProcess.serialSendData[4] = (byte)value;
        }
        if (message.Contains("st1_3rdLED"))
        {
            mainProcess.serialSendData[5] = 133;
            mainProcess.serialSendData[6] = (byte)value;
        }
        mainProcess.SendSerialData();
    }
}
