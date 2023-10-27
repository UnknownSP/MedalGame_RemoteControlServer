using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

//サウンドの処理
public class stationSoundHandler : MonoBehaviour
{
    public AudioSource audioSource_SE;
    public float SE_volume = 0.05f;
    private AudioSource audioSource_MainSE;
    private AudioSource audioSource_MainBGM;
    private AudioSource audioSource_MainJP;

    public AudioClip audio_launch1st_1;
    public AudioClip audio_launch1st_2;
    public AudioClip audio_launch2nd;
    public AudioClip audio_launch3rd;
    public AudioClip audio_out;
    public AudioClip[] audio_up;
    public AudioClip audio_up_1to2;
    public AudioClip audio_up_2to3;
    public AudioClip[] audio_JPC;
    public AudioClip audio_hold;
    public AudioClip audio_JPCBGM_1;
    public AudioClip audio_JPCBGM_2;
    public AudioClip audio_JPCEnd_1;
    public AudioClip audio_JPCEnd_2;
    public AudioClip audio_JPCEnd_3;
    public AudioClip audio_JPIn_1;
    public AudioClip audio_JPIn_2;
    public AudioClip audio_stNum_1;
    public AudioClip audio_stNum_2;
    public AudioClip audio_stNum_3;
    public AudioClip audio_stNum_4;
    public AudioClip audio_Congraturation;

    private int processState = 0;
    private int recent_processState = 0;

    public bool _is_JPC = false;

    private int stationNumber = 1;

    void Start()
    {
    }

    void Update()
    {
        //ステーションの処理状況ごとにサウンド処理
        switch (processState)
        {
            case 0:
                if (recent_processState == 11 || recent_processState == 12)
                {
                    audioSource_MainJP.Stop();
                    audioSource_MainJP.PlayOneShot(audio_JPCEnd_1, 1.0f);
                    Invoke(nameof(Reset_JPCflag), 5.0f);
                }
                if (recent_processState == 13 || recent_processState == 14)
                {
                    audioSource_MainJP.Stop();
                    audioSource_MainJP.PlayOneShot(audio_JPCEnd_2, 1.0f);
                    Invoke(nameof(Reset_JPCflag), 7.0f);
                }
                if (recent_processState == 2 || recent_processState == 3)
                {
                    audioSource_SE.PlayOneShot(audio_out, 0.9f);
                    _is_JPC = false;
                }
                if (recent_processState == 5 || recent_processState == 6)
                {
                    audioSource_SE.PlayOneShot(audio_out, 0.9f);
                    _is_JPC = false;
                }
                if (recent_processState == 8 || recent_processState == 9)
                {
                    audioSource_SE.PlayOneShot(audio_out, 0.9f);
                    _is_JPC = false;
                }
                break;

            case 1:
                if(processState != recent_processState)
                {
                    audioSource_SE.PlayOneShot(audio_launch1st_1, 0.7f);
                }
                break;

            case 4:
            case 7:
                if(processState != recent_processState)
                {
                    audioSource_SE.PlayOneShot(audio_up[UnityEngine.Random.Range(0,audio_up.Length)], 0.8f);
                }
                break;

            case 5:
                if (processState != recent_processState)
                {
                    Invoke(nameof(play_1to2), 0.4f);
                    Invoke(nameof(play_launch2nd), 1.4f);
                }
                break;

            case 8:
                if (processState != recent_processState)
                {
                    Invoke(nameof(play_2to3), 0.5f);
                    Invoke(nameof(play_launch3rd), 1.7f);
                }
                break;

            case 10:
            case 11:
                if (processState != recent_processState && !_is_JPC)
                {
                    audioSource_SE.PlayOneShot(audio_JPC[UnityEngine.Random.Range(0, audio_JPC.Length)]);
                    audioSource_MainBGM.Stop();
                    Invoke(nameof(play_JPCBGM_1), 3.0f);
                }
                _is_JPC = true;
                break;

            case 13:
                if (processState != recent_processState)
                {
                    audioSource_MainSE.PlayOneShot(audio_JPIn_1, 1.0f);
                    audioSource_MainJP.Stop();
                    Invoke(nameof(play_JPCBGM_2), 4.0f);
                }
                break;

            case 15:
                if (processState != recent_processState)
                {
                    audioSource_MainSE.PlayOneShot(audio_JPIn_2, 1.0f);
                    audioSource_MainJP.Stop();
                    Invoke(nameof(play_JPCEnd_3), 4.0f);
                    Invoke(nameof(play_stationNumber), 5.0f);
                    Invoke(nameof(play_congraturation), 7.2f);
                    Invoke(nameof(Reset_JPCflag), 15.0f);
                }
                break;
        }


        recent_processState = processState;
    }

    public void setAudioVolume(float volume)
    {
        SE_volume = volume;
        audioSource_SE.volume = SE_volume;
    }
    public void setAudioSource_MainSE(AudioSource source, float volume)
    {
        audioSource_MainSE = source;
        audioSource_MainSE.volume = volume;
    }
    public void setAudioSource_MainBGM(AudioSource source, float volume)
    {
        audioSource_MainBGM = source;
        audioSource_MainBGM.volume = volume;
    }
    public void setAudioSource_MainJP(AudioSource source, float volume)
    {
        audioSource_MainJP = source;
        audioSource_MainJP.volume = volume;
    }
    public void setStationNumber(int num)
    {
        stationNumber = num;
    }
    public void setProcessState(int state)
    {
        processState = state;
    }

    private void play_1to2()
    {
        audioSource_SE.PlayOneShot(audio_up_1to2, 1.0f);
    }
    private void play_2to3()
    {
        audioSource_SE.PlayOneShot(audio_up_2to3, 0.6f);
    }
    private void play_launch2nd()
    {
        audioSource_SE.PlayOneShot(audio_launch2nd, 0.8f);
    }
    private void play_launch3rd()
    {
        audioSource_SE.PlayOneShot(audio_launch3rd, 0.9f);
    }
    private void play_stationNumber()
    {
        switch (stationNumber)
        {
            case 1:
                audioSource_MainSE.PlayOneShot(audio_stNum_1, 0.8f);
                break;
            case 2:
                audioSource_MainSE.PlayOneShot(audio_stNum_2, 0.8f);
                break;
            case 3:
                audioSource_MainSE.PlayOneShot(audio_stNum_3, 0.8f);
                break;
            case 4:
                audioSource_MainSE.PlayOneShot(audio_stNum_4, 0.8f);
                break;
        }
    }
    private void play_congraturation()
    {
        audioSource_MainSE.PlayOneShot(audio_Congraturation, 0.8f);
    }
    private void play_JPCBGM_1()
    {
        audioSource_MainJP.clip = audio_JPCBGM_1;
        audioSource_MainJP.loop = true;
        audioSource_MainJP.Play();
    }
    private void play_JPCBGM_2()
    {
        audioSource_MainJP.clip = audio_JPCBGM_2;
        audioSource_MainJP.loop = true;
        audioSource_MainJP.Play();
    }
    private void play_JPCEnd_3()
    {
        audioSource_MainJP.clip = audio_JPCEnd_3;
        audioSource_MainJP.loop = false;
        audioSource_MainJP.Play();
    }
    private void Reset_JPCflag()
    {
        _is_JPC = false;
    }
}
