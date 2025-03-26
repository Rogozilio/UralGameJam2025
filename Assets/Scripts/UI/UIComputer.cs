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
    
    public GameObject goose;
    public RectTransform ItchIOPage;
    public RectTransform TelegramPage;
    public RectTransform TelegramScroll;

    public List<ChangeMessage> changeMessages;

    private void OnEnable()
    {
        _screen.LaunchFadeOut(null, 0f);
    }

    public void OpenItchIOPage()
    {
        ItchIOPage.DOAnchorPosX(0, 0.5f);
    }
    
    public void CloseItchIOPage()
    {
        ItchIOPage.DOAnchorPosX(-50, 0.5f);
    }
    
    public void OpenTelegramPage()
    {
        TelegramPage.DOAnchorPosX(0, 0.5f);
    }

    public async void LaunchChangeMassages()
    {
        foreach (var message in changeMessages)
        {
           
            TelegramScroll.DOAnchorPosY(message.positionY, message.time);
            await UniTask.WaitForSeconds(message.wait);
            if (message.activeGoose)
            {
                goose.SetActive(true);
                await UniTask.WaitForSeconds(message.wait);
            }
        }
    }
}
