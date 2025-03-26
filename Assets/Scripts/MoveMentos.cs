using Scripts;
using UnityEngine;
using Zenject;
using Input = Scripts.Input;

public class MoveMentos : MonoBehaviour, IRestart
{
    public Transform target;
    public Transform rigidBody;
    public float speedMove = 0.05f;
    public float speedTurn = 1f;
    public float speedInclination = 0.2f;
    public AudioClip clipPunch;
    
    [Inject] private Input _input;
    [Inject] private ScreenFade _screen;
  

    private Rigidbody _rigidbody;
    private AudioSource _audio;

    private bool _isActiveMove;
    private float _inclinationAngle;
    
    private Vector3 _originPosition;
    private Vector3 _originPositionRig;
    private Vector3 _originEulerAngle;
    private Vector3 _originEulerAngleRig;

    private void Awake()
    {
        _rigidbody = GetComponentInChildren<Rigidbody>();
        _audio = GetComponentInChildren<AudioSource>();

        _originPosition = transform.position;
        _originEulerAngle = transform.eulerAngles;
        _originPositionRig = rigidBody.position;
        _originEulerAngleRig = rigidBody.eulerAngles;
    }

    private void OnEnable()
    {
        _isActiveMove = true;
        _screen.LaunchFadeOut(null, 0f);
        _audio.Play();
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
        _rigidbody.constraints = RigidbodyConstraints.None;
        _rigidbody.useGravity = true;
        _isActiveMove = false;
        _audio.Stop();
        _audio.PlayOneShot(clipPunch);
        
        Restart();
    }

    public void Restart()
    {
        _screen.LaunchFadeIn(() =>
        {
            transform.position = _originPosition;
            transform.eulerAngles = _originEulerAngle;
            rigidBody.position = _originPositionRig;
            rigidBody.eulerAngles = _originEulerAngleRig;

            _inclinationAngle = 0;
            
            _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            _rigidbody.useGravity = false;
            _isActiveMove = true;
            
            _audio.Stop();
            _audio.Play();
            
            _screen.LaunchFadeOut(null, 0f);
        }, 1f);
        
    }
}
