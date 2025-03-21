using System;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour
{
    public Image fadePanel; // Ссылка на панель
    public float fadeDuration = 2f; // Длительность затемнения
    public void LaunchFadeIn(Action onFinish = null)
    {
        StartCoroutine(FadeIn(onFinish));
    }
    
    public void LaunchFadeOut(Action onFinish = null)
    {
        StartCoroutine(FadeOut(onFinish));
    }

    private System.Collections.IEnumerator FadeIn(Action onFinish)
    {
        float elapsedTime = 0f;
        Color panelColor = fadePanel.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.fixedDeltaTime;
            panelColor.a = Mathf.Clamp01(elapsedTime / fadeDuration); // Увеличиваем прозрачность
            fadePanel.color = panelColor;
            yield return null;
        }
        
        onFinish?.Invoke();
    }
    
    private System.Collections.IEnumerator FadeOut(Action onFinish)
    {
        float elapsedTime = fadeDuration;
        Color panelColor = fadePanel.color;

        yield return new WaitForSeconds(0.5f);
        
        while (elapsedTime > 0)
        {
            elapsedTime -= Time.fixedDeltaTime;
            panelColor.a = Mathf.Clamp01(elapsedTime / fadeDuration); // Увеличиваем прозрачность
            fadePanel.color = panelColor;
            yield return null;
        }
        
        onFinish?.Invoke();
    }
}
