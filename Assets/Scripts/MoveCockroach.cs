using Scripts;
using UnityEngine;
using Zenject;

public class MoveCockroach : MonoBehaviour, IRestart
{
    [Inject] private Scripts.Input _input;
    [Inject] private ScreenFade _screenFade;
    [Inject] private GameManager _gameManager;
    
    public float speed = 5f;
    public float rotationSpeed = 10f;
    
    public Animator animator;

    private Rigidbody rb;

    public float raycastDistance = 1.5f; // Дистанция Raycast
    public LayerMask groundLayer; // Слой, на котором ищем поверхность

    private AudioSource _audio;
    
    private Vector3 surfaceNormal; // Нормаль поверхности
    
    private Vector3 _respawnPosition;
    private Quaternion _respawnRotation;

    private void Awake()
    {
        _screenFade.LaunchFadeOut();
        _audio = GetComponent<AudioSource>();

        _respawnPosition = transform.position;
        _respawnRotation = transform.rotation;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        CalculateSurfaceNormal();

        rb.AddForce(-surfaceNormal * 10f, ForceMode.Acceleration);
        
        Move();
    }

    private void Move()
    {
        //_audio.pitch = 1.5f;
        animator.speed = 0f;   
        
        if (_input.PressForwardMove)
        {
            transform.position += transform.forward * Time.fixedDeltaTime * speed;
            animator.speed = 1f;
            if(!_audio.isPlaying)
                _audio.Play();
        }
        if (_input.PressBackMove)
        {
            transform.position -= transform.forward * Time.fixedDeltaTime * speed;
            animator.speed = 1f;
            if(!_audio.isPlaying)
                _audio.Play();
        }
        if (_input.PressLeftMove)
        {
            transform.Rotate(transform.up, -rotationSpeed, Space.World);
            animator.speed = 1f;
            if(!_audio.isPlaying)
                _audio.Play();
        }
        if (_input.PressRightMove)
        {
            transform.Rotate(transform.up, rotationSpeed, Space.World);
            animator.speed = 1f;
            if(!_audio.isPlaying)
                _audio.Play();
        }

        if (!_input.PressForwardMove && !_input.PressBackMove && !_input.PressLeftMove && !_input.PressRightMove)
        {
            _audio.Stop();
        }
    }

    private void CalculateSurfaceNormal()
    {
        Vector3 rayOrigin = transform.position;

        Vector3 rayDirection = -transform.up;

        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, raycastDistance, groundLayer))
        {
            surfaceNormal = hit.normal;
            Debug.DrawRay(hit.point, hit.normal, Color.green); // Визуализация нормали
        }
        else
        {
            surfaceNormal = Vector3.up;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out ChangeVirtualCamera camera))
        {
            camera.IsActiveChange = true;
        }

        if (other.transform.CompareTag("Dead"))
        {
            Restart();
            
        }
        
        if (other.transform.CompareTag("Finish"))
        {
            _gameManager.SwitchGameStep(GameStep.CutsceneCockroach_Plant);
            
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position - transform.up);
    }

    public void Restart()
    {
        _screenFade.LaunchFadeIn(() =>
        {
            transform.position = _respawnPosition;
            transform.rotation = _respawnRotation;
            _screenFade.LaunchFadeOut();
        });
    }
}