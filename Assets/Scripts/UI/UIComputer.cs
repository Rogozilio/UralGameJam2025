using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

[Serializable]
public struct ChangeMessage
{
    public float time;
    public float positionY;
    public float wait;
    public bool activeGoose;
}
public class UIComputer : MonoBehaviour
{
    [Inject] private ScreenFade _screen;
    [Inject] private UIControll _uiControll;
    
    public GameObject goose;
    public RectTransform ItchIOPage;
    public RectTransform TelegramPage;
    public RectTransform TelegramScroll;

    public List<ChangeMessage> changeMessages;

    public AudioClip _clipClick;
    public AudioClip _clipMessage;
    [Space]
    public AudioClip _voiceStart;
    public AudioClip _voiceMissItchIO;
    public AudioClip _voiceFinishItchIO;
    public AudioClip _voiceFinish;

    public AudioSource _audio;
    public AudioSource _audioVoice;

    private bool _isFinishItchIO;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    public void PlayBeginVoice()
    {
        if(_audioVoice.isPlaying)
            _audioVoice.Stop();
        _audioVoice.PlayOneShot(_voiceStart);
    }
    
    public void PlayFinishItchIO()
    {
        _isFinishItchIO = true;
        if(_audioVoice.isPlaying)
            _audioVoice.Stop();
        _audioVoice.PlayOneShot(_voiceFinishItchIO);
    }

    public void OpenItchIOPage()
    {
        ItchIOPage.DOAnchorPosX(0, 0.5f);
        _audio.Stop();
        _audio.PlayOneShot(_clipClick);
        
    }
    
    public void CloseItchIOPage()
    {
        ItchIOPage.DOAnchorPosX(-50, 0.5f);
        _audio.Stop();
        _audio.PlayOneShot(_clipClick);
    }
    
    public void OpenTelegramPage()
    {
        if (!_isFinishItchIO)
        {
            if(_audioVoice.isPlaying)
                _audioVoice.Stop();
            _audioVoice.PlayOneShot(_voiceMissItchIO);
            return;
        }
        TelegramPage.DOAnchorPosX(0, 0.5f);
        _audio.Stop();
        _audio.PlayOneShot(_clipClick);
    }

    public async void LaunchChangeMassages()
    {
        foreach (var message in changeMessages)
        {
           
            TelegramScroll.DOAnchorPosY(message.positionY, message.time);
            _audio.Stop();
            _audio.PlayOneShot(_clipMessage);
            await UniTask.WaitForSeconds(message.wait);
            if (message.activeGoose)
            {
                goose.SetActive(true);
                await UniTask.WaitForSeconds(message.wait);
            }
        }
        
        if(_audioVoice.isPlaying)
            _audioVoice.Stop();
        _audioVoice.PlayOneShot(_voiceFinish);
    }
}
