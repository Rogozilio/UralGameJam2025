using System;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour
{
    public Image fadePanel; // Ссылка на панель
    public float fadeDuration = 2f; // Длительность затемнения

    private bool _isVisible = false;
    public bool isVisible => _isVisible;
    public void LaunchFadeIn(Action onFinish = null, float delay = 0f)
    {
        StartCoroutine(FadeIn(onFinish, delay));
    }
    
    public void LaunchFadeOut(Action onFinish = null, float delay = 0.5f)
    {
        StartCoroutine(FadeOut(onFinish, delay));
    }

    private System.Collections.IEnumerator FadeIn(Action onFinish, float delay)
    {
        if (!isVisible) yield break;
        _isVisible = false;
        float elapsedTime = 0f;
        Color panelColor = fadePanel.color;
        
        yield return new WaitForSeconds(delay);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.fixedDeltaTime;
            panelColor.a = Mathf.Clamp01(elapsedTime / fadeDuration); // Увеличиваем прозрачность
            fadePanel.color = panelColor;
            yield return null;
        }
        
        onFinish?.Invoke();
    }
    
    private System.Collections.IEnumerator FadeOut(Action onFinish, float delay)
    {
        if (isVisible) yield break;
        _isVisible = true;
        float elapsedTime = fadeDuration;
        Color panelColor = fadePanel.color;

        yield return new WaitForSeconds(delay);
        
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
