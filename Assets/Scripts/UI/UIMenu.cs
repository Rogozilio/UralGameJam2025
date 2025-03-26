using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Input = Scripts.Input;

public class UIMenu : MonoBehaviour
{
    [Inject] private Input _input;
    
    public GameObject mainPanel;
    
    public Button btnContinue;
    public Button btnExit;
    public Slider mouseSensitivity;
    public Slider volumeSound;

    private void Awake()
    {
        mainPanel.SetActive(false);
    }

    private void OnEnable()
    {
        _input.OnActionEcs += HideOrShow;
        
        btnContinue.onClick.AddListener(Hide);
        btnExit.onClick.AddListener(ExitGame);
        
        mouseSensitivity.onValueChanged.AddListener(_input.ChangeMouseSensitivity);
    }

    private void OnDisable()
    {
        _input.OnActionEcs -= HideOrShow;
        
        btnContinue.onClick.RemoveListener(Hide);
        btnExit.onClick.RemoveListener(ExitGame);
        
        mouseSensitivity.onValueChanged.RemoveListener(_input.ChangeMouseSensitivity);
    }

    public void Show()
    {
        mainPanel.SetActive(true);
    }
    
    public void Hide()
    {
        mainPanel.SetActive(false);
    }

    private void HideOrShow()
    {
        if (mainPanel.activeSelf)
            Hide();
        else
            Show();
    }

    private void ExitGame()
    {
        Application.Quit();   
    }
}
