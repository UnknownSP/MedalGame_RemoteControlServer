using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using TMPro;

//メイン処理
//サーバ側は画面の送信とシリアル通信のハンドル、サウンド処理を行うだけ
public class MainProcess : MonoBehaviour
{
    public SerialHandler serialHandler;
    public byte[] serialSendData = new byte[8] {
        0,0,0,0,0,0,0,0
    };
    static double systemTime = 0.0;
    public int st1_processState = 0;

    [SerializeField] private AudioSource audioSource_SE;
    [SerializeField] private AudioSource audioSource_BGM;
    [SerializeField] private AudioSource audioSource_JP;

    [SerializeField] private float SE_volume = 0.05f;
    [SerializeField] private float BGM_volume = 0.05f;
    [SerializeField] private float JP_volume = 0.05f;

    public AudioClip audio_JPC;
    public AudioClip[] audio_normalBGM;

    public stationSoundHandler st1_SoundHandler;

    private bool _audioBGM_start = false;
    private bool _is_JPC = false;

    void Start()
    {
        //シリアルハンドラの追加
        serialHandler.OnDataReceived += SerialDataReceived;
        audioSource_SE.volume = SE_volume;
        audioSource_BGM.volume = BGM_volume;
        audioSource_JP.volume = JP_volume;

        st1_SoundHandler.setStationNumber(1);
        st1_SoundHandler.setAudioVolume(SE_volume);
        st1_SoundHandler.setAudioSource_MainBGM(audioSource_BGM, BGM_volume);
        st1_SoundHandler.setAudioSource_MainSE(audioSource_SE, SE_volume);
        st1_SoundHandler.setAudioSource_MainJP(audioSource_JP, JP_volume);
    }

    // Update is called once per frame
    void Update()
    {

        systemTime += Time.deltaTime;
        if(systemTime >= 2.0)
        {
            systemTime = 0.0;
            //デバッグ用
        }

        //サウンドハンドラに処理状況をセット
        st1_SoundHandler.setProcessState(st1_processState);

        if (st1_SoundHandler._is_JPC && !_is_JPC)
        {
            _is_JPC = true;
        }
        else if(!st1_SoundHandler._is_JPC && _is_JPC)
        {
            _is_JPC = false;
            audioSource_JP.Stop();
        }


        if (!_audioBGM_start && !_is_JPC)
        {
            audioSource_BGM.clip = audio_normalBGM[UnityEngine.Random.Range(0, audio_normalBGM.Length)];
            audioSource_BGM.loop = false;
            audioSource_BGM.Play();
            _audioBGM_start = true;
        }
        if (!audioSource_BGM.isPlaying && !_is_JPC)
        {
            audioSource_BGM.clip = audio_normalBGM[UnityEngine.Random.Range(0, audio_normalBGM.Length)];
            audioSource_BGM.Play();
        }


        KeyBoard_Interrupt();
    }


    //シリアル通信で受信した場合
    void SerialDataReceived(string message)
    {
        var receiveData = message.Split(new string[] { "\n" }, System.StringSplitOptions.None);
        string[] st1_serialReceiveData = new string[] { };
        if (receiveData[0] == null) return;
        for (int i=0; i<receiveData.Length; i++)
        {
            if (receiveData[i].Contains("processState:"))
            {
                st1_serialReceiveData = receiveData[i].Split(new string[] { "," }, System.StringSplitOptions.None);
                if (st1_serialReceiveData[1] != null)
                {
                    st1_processState = int.Parse(st1_serialReceiveData[1]);
                    Debug.Log("st1_processState : " + st1_processState);
                }
                //Debug.Log(receiveData[i]);
            }
            if (receiveData[i].Contains("PC:"))
            {
                Debug.Log(receiveData[i]);
            }
        }
        serialHandler.Write(serialSendData, 0, 8);
    }
    public void SendSerialData()
    {
        serialHandler.Write(serialSendData, 0, 8);
    }

    void KeyBoard_Interrupt()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            st1_processState += 1;
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            st1_processState = 0;
        }
    }
}
