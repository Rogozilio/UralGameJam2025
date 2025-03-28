using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;
using Input = Scripts.Input;

public class UIMenu : MonoBehaviour
{
    [Inject] private Input _input;

    public AudioMixer audioMixer;
    public GameObject mainPanel;
    
    public Button btnContinue;
    public Button btnExit;
    public Slider mouseSensitivity;
    public Slider volumeMaster;
    public Slider volumeMusic;
    public Slider volumeVoice;
    public Slider volumeVFX;
    public Slider volumeCutscene;

    public float GetMouseSens => mouseSensitivity.value;

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

    private void Start()
    {
        ChangeVolume();
    }

    private void FixedUpdate()
    {
        if(!mainPanel.activeSelf) return;

        ChangeVolume();
    }

    private void ChangeVolume()
    {
        audioMixer.SetFloat("Master", math.log10(volumeMaster.value) * 20f);
        audioMixer.SetFloat("Music", math.log10(volumeMusic.value) * 20f);
        audioMixer.SetFloat("Voice", math.log10(volumeVoice.value) * 20f);
        audioMixer.SetFloat("VFX", math.log10(volumeVFX.value) * 20f);
        audioMixer.SetFloat("Cutscene", math.log10(volumeCutscene.value) * 20f);
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
