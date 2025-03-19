using UnityEngine;
using UnityEngine.Events;
using Zenject;
using Input = Scripts.Input;

public class Toaster : MonoBehaviour
{
    [Inject] private Input _input;
    
    public Transform arrow;
    public float angleSpeed = 1f;
    public UnityAction onFinishArrow;

    private bool _isBegin;
    private bool _isStop;

    private Collider _collider;

    private void Awake()
    {
        _collider = arrow.GetComponentInChildren<Collider>();
    }

    private void OnEnable()
    {
        _input.OnAction += PrepareOrLaunchArrow;
    }

    private void OnDisable()
    {
        _input.OnAction -= PrepareOrLaunchArrow;
    }

    private void FixedUpdate()
    {
        if (_isBegin && !_isStop)
        {
            arrow.Rotate(transform.forward, -angleSpeed, Space.Self);
        }
    }

    public void PrepareOrLaunchArrow()
    {
        if (!_isBegin)
        {
            _isBegin = true;
            return;
        }

        if (!_isStop)
        {
            _isStop = true;
            _collider.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Finish"))
        {
            Debug.Log("Finish");
            onFinishArrow?.Invoke();
        }
    }
}
