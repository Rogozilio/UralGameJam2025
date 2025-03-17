using UnityEngine;
using Zenject;

public class MoveLadybug : MonoBehaviour
{
    [Inject] private Scripts.Input _input;
    
    public Transform vectorMove;
    public float speed = 5f;
    public float rotationSpeed = 10f;

    private Rigidbody rb;

    public float raycastDistance = 1.5f; // Дистанция Raycast
    public LayerMask groundLayer; // Слой, на котором ищем поверхность

    private Vector3 surfaceNormal; // Нормаль поверхности

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        CalculateSurfaceNormal();

        rb.AddForce(-surfaceNormal * 10f, ForceMode.Acceleration);

        vectorMove.right = Vector3.forward;
        vectorMove.up = surfaceNormal;
        
        Move();
    }

    private void Move()
    {
        if (_input.PressForwardMove || _input.PressLeftMove || _input.PressRightMove || _input.PressBackMove)
        {
            rb.MovePosition(transform.position + (transform.forward * speed * Time.deltaTime));
        }

        if (_input.PressForwardMove)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-vectorMove.right),
                rotationSpeed * Time.deltaTime);
        }
        else if (_input.PressBackMove)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vectorMove.right),
                rotationSpeed * Time.deltaTime);
        }
        
        if (_input.PressRightMove)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vectorMove.forward),
                rotationSpeed * Time.deltaTime);
        }
        else if (_input.PressLeftMove)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-vectorMove.forward),
                rotationSpeed * Time.deltaTime);
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

        Debug.Log("Surface Normal: " + surfaceNormal);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position - transform.up);
    }
}