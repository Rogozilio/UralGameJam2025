using System;
using UnityEngine;
using Zenject;
using Input = Scripts.Input;

public class MentosFall : MonoBehaviour
{
    [Inject] private ScreenFade _screen;
    [Inject] private Input _input;

    public float speed;
    public float speedRotate;
    public float speedGravity;
    public Transform pointMoveDir;
    [Space] 
    public AudioClip udarObstacle;

    private Rigidbody _rb;
    private Vector3 _originPosition;
    private Vector3 _originEulerAngle;

    private AudioSource _audio;

    private bool _isPlayingAudioObstacle;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _audio = GetComponent<AudioSource>();

        _originPosition = transform.position;
        _originEulerAngle = transform.eulerAngles;
    }

    private void OnEnable()
    {
        _screen.LaunchFadeOut(null, 0f);
        _audio.Play();
    }

    private void FixedUpdate()
    {
        _rb.AddForce(Vector3.down * speedGravity, ForceMode.Acceleration);
        
        if (_input.PressForwardMove)
        {
            _rb.MovePosition(transform.position + pointMoveDir.forward * speed * Time.fixedDeltaTime);
            transform.Rotate(pointMoveDir.right, speedRotate, Space.World);
        }
        
        if (_input.PressBackMove)
        {
            _rb.MovePosition(transform.position - pointMoveDir.forward * speed * Time.fixedDeltaTime);
            transform.Rotate(pointMoveDir.right, -speedRotate, Space.World);
        }
        
        if (_input.PressLeftMove)
        {
            _rb.MovePosition(transform.position - pointMoveDir.right * speed * Time.fixedDeltaTime);
            transform.Rotate(pointMoveDir.forward, speedRotate, Space.World);
        }
        
        if (_input.PressRightMove)
        {
            _rb.MovePosition(transform.position + pointMoveDir.right * speed * Time.fixedDeltaTime);
            transform.Rotate(pointMoveDir.forward, -speedRotate, Space.World);
        }
    }

    private void Restart()
    {
        _isPlayingAudioObstacle = false;
        transform.position = _originPosition;
        transform.eulerAngles = _originEulerAngle;
        _rb.velocity = Vector3.zero;
        _audio.Play();
        _screen.LaunchFadeOut(null, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Dead"))
        {
            _screen.LaunchFadeIn(Restart, 0f);
        }

        if (other.gameObject.CompareTag("Finish"))
        {
            
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            if (!_isPlayingAudioObstacle)
            {
                _audio.PlayOneShot(udarObstacle);
                _isPlayingAudioObstacle = true;
            }
            
            _screen.LaunchFadeIn(Restart, 0f);
        }
    }
}
