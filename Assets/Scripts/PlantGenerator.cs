using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using Input = Scripts.Input;

public class PlantGenerator : MonoBehaviour
{
    [Inject] private Input _input;
    
    public Transform pointGrow;
    public float speedGrowUp = 1f;
    public float forceLeft = -0.1f;
    public float stepForceLeft = -0.01f;
    public float stepForceRight = 0.04f;
    
    [HideInInspector] public List<Vector3> points; // Массив точек

    public UnityEvent onEnterFinishZone;
    public UnityEvent onEnterDeadZone;

    void Awake()
    {
        points = new List<Vector3>();
        AddPoint(pointGrow.position);
        AddPoint(pointGrow.position);
    }

    private void OnEnable()
    {
        _input.OnAction += ChangeForceLeftToRight;
    }

    private void OnDisable()
    {
        _input.OnAction -= ChangeForceLeftToRight;
    }

    private void FixedUpdate()
    {
        forceLeft += stepForceLeft;
        forceLeft = math.clamp(forceLeft, -0.15f, 0.15f);
        
        pointGrow.position += new Vector3(forceLeft, speedGrowUp, 0f) * Time.fixedDeltaTime;

        UpdateLastPoint(pointGrow.position);
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
        if (pointGrow.position.y - points[^2].y > 0.1f)
        {
            Debug.Log("AddPoint");
            AddPoint(pointGrow.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Finish"))
        {
            enabled = false;
        }
        
        if (other.transform.CompareTag("Respawn"))
        {
            enabled = false;
            onEnterDeadZone?.Invoke();
        }
    }
}
