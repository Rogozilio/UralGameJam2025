using System.Collections;
using Cysharp.Threading.Tasks;
using Scripts;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using Input = Scripts.Input;

public class Toaster : MonoBehaviour, IRestart
{
    [Inject] private Input _input;
    [Inject] private ScreenFade _screen;
    [Inject] private GameManager _gameManager;
    [Inject] private UIControll _uiControll;

    public Animator animator;
    public ParticleSystem particleFail;
    public Transform arrow;
    public float angleSpeed = 1f;
    public float millisecondsForBegin = 1f;
    [Space] public AudioClip clipFailBeforeFinish;
    public AudioClip clipFailAfterFinish;
    public AudioClip clipFinish;

    private bool _isBegin;
    private bool _isStop;

    private Collider _collider;
    private AudioSource _audio;

    private Vector3 _originLocalRotate;
    private float _angleRotate;

    private void Awake()
    {
        _collider = arrow.GetComponentInChildren<Collider>();
        _audio = arrow.GetComponentInChildren<AudioSource>();
    }

    private void OnEnable()
    {
        _uiControll.EnableToaster();
        _originLocalRotate = arrow.localEulerAngles;
        _input.OnAction += PrepareOrLaunchArrow;

        if (!_screen.isVisible)
            _screen.LaunchFadeOut(LaunchStartToasterMinigame);
        else
            LaunchStartToasterMinigame();
    }

    public void LaunchStartToasterMinigame()
    {
        StartCoroutine(StartToasterMinigame());
    }

    private IEnumerator StartToasterMinigame()
    {
        yield return new WaitForSeconds(millisecondsForBegin);

        _isBegin = true;
    }

    private void OnDisable()
    {
        _input.OnAction -= PrepareOrLaunchArrow;
    }

    private void FixedUpdate()
    {
        if (_isBegin && !_isStop)
        {
            arrow.Rotate(arrow.forward, -angleSpeed, Space.World);
            _angleRotate += angleSpeed;

            if (_angleRotate > 245f)
            {
                FailAfterFinish();
            }
        }
    }

    public void PrepareOrLaunchArrow()
    {
        if (!_isStop && _isBegin)
        {
            _isStop = true;
            _collider.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Finish"))
        {
            Finish();
        }
        else if (other.transform.CompareTag("Dead"))
        {
            FailAfterFinish();
        }
        else if (other.transform.CompareTag("Obstacle"))
        {
            FailBeforeFinish();
        }
    }

    private async void Finish()
    {
        Debug.Log("Finish");
        _audio.clip = clipFinish;
        _audio.Play();
        _collider.enabled = false;
        animator.Play("ToasterFinish");
        _uiControll.DisableAll();
        await UniTask.WaitForSeconds(0.3f);
        _gameManager.SwitchGameStep(GameStep.CutsceneToaster_Cockroach);
    }

    private void FailAfterFinish()
    {
        Debug.Log("Dead");
        _isStop = true;
        _audio.clip = clipFailAfterFinish;
        _audio.Play();
        _collider.enabled = false;
        particleFail.Play();
        Restart();
    }

    private void FailBeforeFinish()
    {
        Debug.Log("Before Dead");
        _isStop = true;
        _audio.clip = clipFailBeforeFinish;
        _audio.Play();
        _collider.enabled = false;
        animator.Play("ToasterFail");
        Restart();
    }

    public void Restart()
    {
        _screen.LaunchFadeIn(() =>
        {
            _isBegin = false;
            _isStop = false;
            _angleRotate = 0f;
            arrow.localEulerAngles = _originLocalRotate;
            animator.Play("ToasterDefault");
            particleFail.Stop();
            particleFail.Clear();
            _screen.LaunchFadeOut(LaunchStartToasterMinigame);
        }, 3f);
    }

    public void FadeIn()
    {
        
    }
}