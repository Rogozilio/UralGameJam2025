using System;
using UnityEngine;
using Zenject;

public class MoveCockroach : MonoBehaviour
{
    [Inject] private Scripts.Input _input;
    [Inject] private ScreenFade _screenFade;
    
    public float speed = 5f;
    public float rotationSpeed = 10f;
    
    public Animator animator;

    private Rigidbody rb;

    public float raycastDistance = 1.5f; // Дистанция Raycast
    public LayerMask groundLayer; // Слой, на котором ищем поверхность

    private Vector3 surfaceNormal; // Нормаль поверхности
    
    private Vector3 _respawnPosition;
    private Quaternion _respawnRotation;

    private void Awake()
    {
        _screenFade.LaunchFadeOut();

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
        animator.speed = 0f;   
        if (_input.PressForwardMove)
        {
            transform.position += transform.forward * Time.fixedDeltaTime * speed;
            animator.speed = 1f;
        }
        if (_input.PressBackMove)
        {
            transform.position -= transform.forward * Time.fixedDeltaTime * speed;
            animator.speed = 1f;
        }
        if (_input.PressLeftMove)
        {
            transform.Rotate(transform.up, -rotationSpeed, Space.World);
            animator.speed = 1f;
        }
        if (_input.PressRightMove)
        {
            transform.Rotate(transform.up, rotationSpeed, Space.World);
            animator.speed = 1f;
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
            _screenFade.LaunchFadeIn(() =>
            {
                Restart();
                _screenFade.LaunchFadeOut();
            });
            
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position - transform.up);
    }

    private void Restart()
    {
        transform.position = _respawnPosition;
        transform.rotation = _respawnRotation;
    }
}