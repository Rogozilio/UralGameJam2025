using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class MentosGun : MonoBehaviour
{
    [Inject] private ScreenFade _screen;

    public bool isActive;
    public GameObject virtualCamera;
    public Collider finishZone;
    [Space] public Transform mentosGraphic;
    public float speedRotateMentos = 5f;
    public float scale = 0.3f;
    [Space] public Transform targetPosition; // Целевая позиция
    public float height = 5f; // Высота траектории
    public float duration = 2f; // Время полета
    public LineRenderer lineRenderer; // Компонент LineRenderer
    public int resolution = 30; // Количество точек на траектории
    [Space] public float minDistanceCurve = 1f;
    public float maxDistanceCurve = 10f;
    public float deltaDistanceCurve = 1f;
    public float deltaRotateCurve = 3f;
    [Space] public AudioSource audioСola;
    public AudioSource audio;
    public AudioClip launch;
    public AudioClip finish;

    [Inject] private Scripts.Input _input;
    [Inject] private GameManager _gameManager;

    private Rigidbody _rigidbody;
    private Vector3 startPosition;
    private Vector3 originStartPosition;
    private float linearMove;
    private float elapsedTime;
    private bool isThrown;
    private bool isDisabled;

    public bool IsActive
    {
        set
        {
            isActive = value;
            OnValidate();
        }
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        RefreshStartPosition();
        originStartPosition = transform.position;
    }

    private void OnEnable()
    {
        _screen.LaunchFadeOut();
        _input.OnAction += Fire;
        transform.localScale = new Vector3(scale, scale, scale);
        audioСola.Play();
    }

    private void OnDisable()
    {
        _input.OnAction -= Fire;
    }

    private void OnValidate()
    {
        if (isActive)
        {
            virtualCamera.SetActive(true);
            enabled = true;
            finishZone.enabled = false;
        }

        if (!isActive)
        {
            virtualCamera.SetActive(false);
            enabled = false;
            finishZone.enabled = true;
        }
    }

    void FixedUpdate()
    {
        if (!isThrown && !isDisabled)
        {
            MoveTarget();
            DrawTrajectory();
        }

        if (isThrown)
        {
            mentosGraphic.gameObject.SetActive(true);
            lineRenderer.positionCount = 0;
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
                //isThrown = false; // Завершаем движение
                isDisabled = true;
                _rigidbody.useGravity = true;
                RefreshStartPosition();
            }
        }

        if (isDisabled)
        {
            targetPosition.position = transform.position;
        }
    }

    private void Fire()
    {
        if (!isThrown)
        {
            audio.clip = launch;
            audio.Play();
            elapsedTime = 0f;
            RefreshStartPosition();
        }

        isThrown = true;
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
        currentPos.y += y - 0.02f; // Добавляем высоту
        return currentPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            if (!enabled) return;
            if (other.TryGetComponent(out ActiveGameObject activeGameObject))
            {
                activeGameObject.Action?.Invoke();
                virtualCamera.SetActive(false);
                gameObject.SetActive(false);
            }
        }
        else if (other.CompareTag("Dead"))
        {
            _screen.LaunchFadeIn(Restart);
        }
    }

    public void Finish()
    {
        _gameManager.SwitchGameStep(GameStep.СutsceneMentosGun_Toaster);
    }

    private void Restart()
    {
        isThrown = false;
        isDisabled = false;
        elapsedTime = 0f;
        transform.position = originStartPosition;
        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        mentosGraphic.gameObject.SetActive(false);
        audioСola.Play();
        RefreshStartPosition();
        _screen.LaunchFadeOut();
    }
}