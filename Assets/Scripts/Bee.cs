using System;
using Cinemachine;
using Scripts;
using UnityEngine;
using Zenject;
using Input = Scripts.Input;

public class Bee : MonoBehaviour, IRestart
{
    [Inject] private Input _input;
    [Inject] private ScreenFade _screenFade;
    [Inject] private GameManager _gameManager;
    [Inject] private UIMenu _menu;
    [Inject] private UIControll _uiControll;
    
    public float speedInclination = 0.2f;

    public Transform target;
    public Transform model;
    public Rigidbody rb;

    public CinemachineFreeLook camera;

    private AudioSource _audio;
    
    private float _inclinationAngle;
    private float _inclinationAngleModel;

    private Vector3 _originPosition;
    private Vector3 _originEulerAngle;

    private float _originSpeedX;
    private float _originSpeedY;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _originPosition = target.position;
        _originEulerAngle = target.eulerAngles;

        _originSpeedX = camera.m_XAxis.m_MaxSpeed;
        _originSpeedY = camera.m_YAxis.m_MaxSpeed;
    }

    private void OnEnable()
    {
        _audio.Play();
        _uiControll.EnableBee();
        _screenFade.LaunchFadeOut(null, 0f);
    }

    private void FixedUpdate()
    {
        MoveSystem();
        RotateSystem();
        
        camera.m_XAxis.m_MaxSpeed = _originSpeedX * _menu.GetMouseSens;
        camera.m_YAxis.m_MaxSpeed = _originSpeedY * _menu.GetMouseSens;
    }

    private void MoveSystem()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (_input.PressForwardMove)
        {
            rb.velocity = target.forward;
            _inclinationAngleModel += speedInclination;
        }

        if (_input.PressBackMove)
        {
            rb.velocity = -target.forward;
            _inclinationAngleModel -= speedInclination;
        }
        
        if (!_input.PressForwardMove && !_input.PressBackMove)
        {
            _inclinationAngleModel += _inclinationAngleModel > 0 ? -speedInclination : speedInclination;
            if (_inclinationAngleModel <= 2f && _inclinationAngleModel >= -2f)
                _inclinationAngleModel = 0f;
        }
        
        _inclinationAngle = Mathf.Clamp(_inclinationAngle, -25, 25);

        var localEulerRotate = model.localEulerAngles;
        localEulerRotate.z = _inclinationAngle;
        model.localEulerAngles = localEulerRotate;
        
        _inclinationAngleModel = Mathf.Clamp(_inclinationAngleModel, -25, 25);
        
        var localEulerRotateModel = model.localEulerAngles;
        localEulerRotateModel.x = _inclinationAngleModel;
        model.localEulerAngles = localEulerRotateModel;
    }

    private void RotateSystem()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraUp = Camera.main.transform.up;
        
        Quaternion targetRotation = Quaternion.LookRotation(cameraForward, cameraUp);
        
        target.transform.rotation = targetRotation;
    }

    public void Restart()
    {
        _screenFade.LaunchFadeIn(() =>
        {
            _audio.Stop();
            _audio.Play();
            target.position = _originPosition;
            target.eulerAngles = _originPosition;
            _screenFade.LaunchFadeOut(null, 0f);
        });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Finish"))
        {
            _uiControll.DisableAll();
            _audio.Stop();
            _gameManager.SwitchGameStep(GameStep.CutsceneBee_Computer);
            model.gameObject.SetActive(false);
        }
    }
}