using System;
using Scripts;
using UnityEngine;
using Zenject;
using Input = Scripts.Input;

public class Bee : MonoBehaviour, IRestart
{
    [Inject] private Input _input;
    [Inject] private ScreenFade _screenFade;
    [Inject] private GameManager _gameManager;
    
    public float speedInclination = 0.2f;

    public Transform target;
    public Transform model;
    public Rigidbody rb;

    private AudioSource _audio;
    
    private float _inclinationAngle;
    private float _inclinationAngleModel;

    private Vector3 _originPosition;
    private Vector3 _originEulerAngle;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _originPosition = target.position;
        _originEulerAngle = target.eulerAngles;
    }

    private void OnEnable()
    {
        _audio.Play();
        _screenFade.LaunchFadeOut(null, 0f);
    }

    private void FixedUpdate()
    {
        MoveSystem();
        RotateSystem();
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

        // if (_input.PressLeftMove)
        // {
        //     rb.velocity = -target.right;
        //     _inclinationAngle += speedInclination;
        // }
        //
        // if (_input.PressRightMove)
        // {
        //     rb.velocity = target.right;
        //     _inclinationAngle -= speedInclination;
        // }
        
        // if (!_input.PressRightMove && !_input.PressLeftMove)
        // {
        //     _inclinationAngle += _inclinationAngle > 0 ? -speedInclination : speedInclination;
        //     if (_inclinationAngle <= 0.5f && _inclinationAngle >= -0.5f)
        //         _inclinationAngle = 0f;
        // }
        
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
            _gameManager.SwitchGameStep(GameStep.CutsceneBee_Computer);
        }
    }
}