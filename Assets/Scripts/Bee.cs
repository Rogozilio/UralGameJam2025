using UnityEngine;
using Zenject;
using Input = Scripts.Input;

public class Bee : MonoBehaviour
{
    [Inject] private Input _input;
    
    public float speedInclination = 0.2f;

    public Transform target;
    public Transform model;
    public Rigidbody rb;
    
    private float _inclinationAngle;
    private float _inclinationAngleModel;

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

        if (_input.PressLeftMove)
        {
            rb.velocity = -target.right;
            _inclinationAngle += speedInclination;
        }

        if (_input.PressRightMove)
        {
            rb.velocity = target.right;
            _inclinationAngle -= speedInclination;
        }
        
        if (!_input.PressRightMove && !_input.PressLeftMove)
        {
            _inclinationAngle += _inclinationAngle > 0 ? -speedInclination : speedInclination;
            if (_inclinationAngle <= 0.5f && _inclinationAngle >= -0.5f)
                _inclinationAngle = 0f;
        }
        
        if (!_input.PressForwardMove && !_input.PressBackMove)
        {
            _inclinationAngleModel += _inclinationAngleModel > 0 ? -speedInclination : speedInclination;
            if (_inclinationAngleModel <= 0.5f && _inclinationAngleModel >= -0.5f)
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
}