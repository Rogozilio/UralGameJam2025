using System;
using Scripts;
using UnityEngine;
using Zenject;
using Input = Scripts.Input;

public class MentosFall : MonoBehaviour, IRestart
{
    [Inject] private ScreenFade _screen;
    [Inject] private Input _input;
    [Inject] private GameManager _gameManager;

    public float speed;
    public float speedRotate;
    public float speedGravity;
    public Transform pointMoveDir;
    [Space] public AudioClip udarObstacle;

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
        //_screen.LaunchFadeOut(null, 0f);
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

    public void Restart()
    {
        _screen.LaunchFadeIn((() =>
        {
            _isPlayingAudioObstacle = false;
            transform.position = _originPosition;
            transform.eulerAngles = _originEulerAngle;
            _rb.velocity = Vector3.zero;
            _audio.Play();
            _screen.LaunchFadeOut(null, 0f);
        }), 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Dead"))
        {
            Restart();
        }

        if (other.gameObject.CompareTag("Finish"))
        {
            _gameManager.SwitchGameStep(GameStep.CutsceneMentosFall_MentosGun);
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