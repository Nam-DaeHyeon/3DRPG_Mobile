using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private void Awake()
    {
        instance = this;
    }

    [SerializeField] AudioSource _BGMSource;
    [SerializeField] AudioSource _SESource;

    AudioClip _bgmClip;
    AudioClip _seClip;

    private void Play_BGM()
    {
        if (_bgmClip == null)
        {
            _BGMSource.Stop();
            return;
        }
        _BGMSource.clip = _bgmClip;
        _BGMSource.Play();
    }

    private void Play_SE()
    {
        if (_seClip == null)
        {
            _SESource.Stop();
            return;
        }
        _SESource.clip = _seClip;
        _SESource.Play();
    }

    /// <summary>타이틀 배경음악 재생</summary>
    public void Play_BG_Title()
    {
        _bgmClip = Resources.Load<AudioClip>("Audio/BGM_Title");
        Play_BGM();
    }

    /// <summary>로비 배경음악 재생</summary>
    public void Play_BG_Lobby()
    {
        _bgmClip = Resources.Load<AudioClip>("Audio/BGM_Lobby");
        Play_BGM();
    }

    /// <summary>메인 배경음악 재생</summary>
    public void Play_BG_Main()
    {
        _bgmClip = Resources.Load<AudioClip>("Audio/BGM_Main");
        Play_BGM();
    }
    
    /// <summary>
    /// 입력된 키워드로 효과음을 재생합니다.
    /// </summary>
    /// <param name="clipName">Resources/Audio/CLIPNAME</param>
    public void Play_SE_Keyword(string clipName)
    {
        _seClip = Resources.Load<AudioClip>("Audio/SE_" + clipName);
        if (_seClip == null) return;
        Play_SE();
    }
}
