using UnityEngine;
using Zenject;
using Input = Scripts.Input;

public class MentosFall : MonoBehaviour
{
    [Inject] private Input _input;

    public float speed;
    public float speedRotate;
    public float speedGravity;
    public Transform pointMoveDir;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
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
}
