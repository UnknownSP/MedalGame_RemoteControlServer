using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TCP�R���g���[������
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
        //�N���C�A���g�Ɛڑ����Ă��Ȃ��ꍇ�͏������Ȃ�
        if (!TCPServer._connected) return;

        //TCP�Ŏ�M
        receiveTCP = TCPServer.ReceiveData().Split(new string[] { ":" }, System.StringSplitOptions.None);
        //���b�Z�[�W�̒��Ƀe�X�g���[�h�I���������Ă����ꍇ
        for (int i = 0; i < receiveTCP.Length; i++)
        {
            if (receiveTCP[i].Contains("st1_ResetTestMode"))
            {
                for (int j = 0; j < 8; j++)
                {
                    //�V���A���ʐM�ő���f�[�^�����Z�b�g
                    mainProcess.serialSendData[j] = 0;
                }
                Debug.Log("ResetTestMode");
                return;
            }
        }

        //TCP��M�T�C�Y�Ń��[�v
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

        //�N���C�A���g���甭�ˏ����������ꍇ�A�i�s�󋵂ɂ���ă��Z�b�g��������
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
            mainProcess.serialSendData[0] = 100; //100�͎b��̐ݒ�l
        }
        if (message.Contains("st1_1stLED"))
        {
            mainProcess.serialSendData[1] = 131; //131,132,133��LED�e�X�g�̎b��̐ݒ�l
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
