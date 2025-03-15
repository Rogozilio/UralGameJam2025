using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using Input = Scripts.Input;

public class MoveMentos : MonoBehaviour
{
    public Transform target;
    public float speedMove = 0.05f;
    public float speedTurn = 1f;
    public float speedInclination = 0.2f;
    
    [Inject] private Input _input;

    private Rigidbody _rigidbody;

    private bool _isActiveMove;
    private float _inclinationAngle;

    private void Awake()
    {
        _isActiveMove = true;
        _rigidbody = GetComponentInChildren<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if(!_isActiveMove) return;
       
        transform.position += transform.right * speedMove;

        if (_input.PressRightMove)
        {
            transform.Rotate(transform.up, speedTurn, Space.World);
            _inclinationAngle += speedInclination;
        }
        
        if (_input.PressLeftMove)
        {
            transform.Rotate(transform.up, -speedTurn, Space.World);
            _inclinationAngle -= speedInclination;
        }

        if (!_input.PressRightMove && !_input.PressLeftMove)
        {
            _inclinationAngle += _inclinationAngle > 0 ? -speedInclination : speedInclination;
            if (_inclinationAngle <= 0.5f && _inclinationAngle >= -0.5f)
                _inclinationAngle = 0f;
        }

        _inclinationAngle = Mathf.Clamp(_inclinationAngle, -10, 10);

        var localEulerRotate = transform.eulerAngles;
        localEulerRotate.x = _inclinationAngle;
        localEulerRotate.z = 0;
        transform.eulerAngles = localEulerRotate;
        
        target.Rotate(transform.forward, 5, Space.World);
    }

    public void EnableRigidbody()
    {
        _rigidbody.isKinematic = false;
        _rigidbody.constraints = RigidbodyConstraints.None;
        _rigidbody.useGravity = true;
        _isActiveMove = false;
    }
}
