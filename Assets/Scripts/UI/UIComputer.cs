using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

[Serializable]
public struct ChangeMessage
{
    public float time;
    public Vector3 position;
    public float wait;
    public bool activeGoose;
}
public class UIComputer : MonoBehaviour
{
    public GameObject goose;
    public RectTransform ItchIOPage;
    public RectTransform TelegramPage;
    public RectTransform TelegramScroll;

    public List<ChangeMessage> changeMessages;
    
    public void OpenItchIOPage()
    {
        ItchIOPage.DOMove(new Vector3(-50 , 0, 0), 0.5f);
    }
    
    public void CloseItchIOPage()
    {
        ItchIOPage.DOMove(new Vector3(-100 , 0, 0), 0.5f);
    }
    
    public void OpenTelegramPage()
    {
        TelegramPage.DOMove(TelegramPage.position + new Vector3(85 , 0, 0), 0.5f);
    }

    public async void LaunchChangeMassages()
    {
        Debug.Log("asdasdasd");
        foreach (var message in changeMessages)
        {
           
            TelegramScroll.DOMove(TelegramScroll.position + message.position, message.time);
            await UniTask.WaitForSeconds(message.wait);
            if (message.activeGoose)
            {
                goose.SetActive(true);
                await UniTask.WaitForSeconds(message.wait);
            }
        }
    }
}
