using Unity.Mathematics;
using UnityEngine;
using Zenject;

public class TrajectoryPredictor : MonoBehaviour
{
    public Transform mentosGraphic;
    public float speedRotateMentos = 5f;
    [Space]
    public Transform targetPosition; // Целевая позиция
    public float height = 5f; // Высота траектории
    public float duration = 2f; // Время полета
    public LineRenderer lineRenderer; // Компонент LineRenderer
    public int resolution = 30; // Количество точек на траектории
    [Space] 
    public float minDistanceCurve = 1f;
    public float maxDistanceCurve = 10f;
    public float deltaDistanceCurve = 1f;
    public float deltaRotateCurve = 3f;

    [Inject] private Scripts.Input _input;

    private Vector3 startPosition;
    private float linearMove;
    private float elapsedTime;
    private bool isThrown;

    private void Awake()
    {
        RefreshStartPosition();
    }

    private void OnEnable()
    {
        _input.OnAction += Fire;
    }

    private void OnDisable()
    {
        _input.OnAction -= Fire;
    }

    void FixedUpdate()
    {
        if (!isThrown)
        {
            MoveTarget();
        }
        
        DrawTrajectory();

        if (isThrown)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime < duration)
            {
                // Вычисляем текущую позицию по параболической траектории
                float t = elapsedTime / duration;
                Vector3 currentPos = CalculateParabolicPosition(t);
                transform.position = currentPos;
                mentosGraphic.Rotate(Vector3.right, speedRotateMentos, Space.World);
            }
            else
            {
                isThrown = false; // Завершаем движение
                RefreshStartPosition();
            }
        }
    }

    private void Fire()
    {
        isThrown = true;
        elapsedTime = 0f;
        RefreshStartPosition();
    }

    private void MoveTarget()
    {
        linearMove += math.clamp(_input.deltaMovePosition.y, -deltaDistanceCurve, deltaDistanceCurve);
        linearMove = math.clamp(linearMove, minDistanceCurve, maxDistanceCurve);
        
        targetPosition.position = transform.position + transform.forward * linearMove;

        var angleRotate = math.clamp(_input.deltaMovePosition.x, -deltaRotateCurve, deltaRotateCurve);
        transform.Rotate(Vector3.up, angleRotate, Space.World);
    }

    private void RefreshStartPosition()
    {
        startPosition = transform.position; // Запоминаем начальную позицию
    }
    

    void DrawTrajectory()
    {
        // Устанавливаем количество точек в LineRenderer
        lineRenderer.positionCount = resolution;

        // Рассчитываем позиции для каждой точки
        for (int i = 0; i < resolution; i++)
        {
            float t = (float)i / (resolution - 1);
            Vector3 point = CalculateParabolicPosition(t);
            lineRenderer.SetPosition(i, point);
        }
    }

    Vector3 CalculateParabolicPosition(float t)
    {
        // Параболическая траектория
        float y = height * Mathf.Sin(Mathf.PI * t); // Высота изменяется по синусоиде
        Vector3 currentPos = Vector3.Lerp(startPosition, targetPosition.position, t); // Линейное движение по X и Z
        currentPos.y += y; // Добавляем высоту
        return currentPos;
    }
}
