using System.Collections.Generic;
using Scripts;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using Input = Scripts.Input;

public class PlantGenerator : MonoBehaviour, IRestart
{
    [Inject] private ScreenFade _screen;
    [Inject] private Input _input;
    [Inject] private GameManager _gameManager;

    public ParticleSystem pilca;
    public MeshFilter mesh;
    public Animator animator;
    public Transform pointGrow;
    public float speedGrowUp = 1f;
    public float forceLeft = -0.1f;
    public float stepForceLeft = -0.01f;
    public float stepForceRight = 0.04f;
    
    [HideInInspector] public List<Vector3> points; // Массив точек

    public UnityEvent onEnterFinishZone;
    public UnityEvent onEnterDeadZone;

    public AudioClip _clipPunch;
    public AudioClip _clipFinish;
    
    private AudioSource _audio;

    private float _originForceLeft;
    private Vector3 _originPosition;
    private Vector3 _originEulerRotate;

    void Awake()
    {
        _audio = GetComponent<AudioSource>();
        
        points = new List<Vector3>();
        AddPoint(pointGrow.localPosition);
        AddPoint(pointGrow.localPosition);

        _originForceLeft = forceLeft;
        _originPosition = transform.position;
        _originEulerRotate = transform.eulerAngles;
    }

    private void OnEnable()
    {
        _input.OnAction += ChangeForceLeftToRight;
        _screen.LaunchFadeOut(null, 0f);
        _audio.Play();
    }

    private void OnDisable()
    {
        _input.OnAction -= ChangeForceLeftToRight;
    }

    private void FixedUpdate()
    {
        pointGrow.up = 
            Quaternion.Euler(0, 90f, 0) * (points[^1] - points[^2]).normalized;
        
        forceLeft += stepForceLeft;
        forceLeft = math.clamp(forceLeft, -0.15f, 0.15f);
        
        pointGrow.localPosition += new Vector3(forceLeft, speedGrowUp, 0f) * Time.fixedDeltaTime;

        UpdateLastPoint(pointGrow.localPosition);
        AutoAddPoint();
    }

    private void ChangeForceLeftToRight()
    {
        forceLeft += stepForceRight;
    }

    private void UpdateLastPoint(Vector3 point)
    {
        points[^1] = point;
    }
    
    private void AddPoint(Vector3 point)
    {
        points.Add(point);
    }

    private void AutoAddPoint()
    {
        if (pointGrow.localPosition.y - points[^2].y > 0.1f)
        {
            Debug.Log("AddPoint");
            AddPoint(pointGrow.localPosition);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Finish"))
        {
            pilca.Play();
            _audio.Stop();
            _audio.pitch = 0.4f;
            _audio.PlayOneShot(_clipFinish);
            pointGrow.up = Vector3.up;
            animator.Play("Bloom");
            enabled = false;
            _gameManager.SwitchGameStep(GameStep.CutscenePlant_Bee, 5f);
        }
        
        if (other.transform.CompareTag("Respawn"))
        {
            _audio.PlayOneShot(_clipPunch);
            Restart();
            enabled = false;
            onEnterDeadZone?.Invoke();
        }
    }

    public void Restart()
    {
        _screen.LaunchFadeIn(() =>
        {
            enabled = true;
            forceLeft = _originForceLeft;
            transform.position = _originPosition;
            transform.eulerAngles = _originEulerRotate;

            mesh.mesh = new Mesh();

            points = new List<Vector3>();
            AddPoint(pointGrow.localPosition);
            AddPoint(pointGrow.localPosition);

            _audio.Stop();
            _audio.Play();

            _screen.LaunchFadeOut(null, 0f);
        });
    }
}
