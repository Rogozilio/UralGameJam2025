using Unity.Mathematics;
using UnityEngine;
using Zenject;
using Input = Scripts.Input;

public class Bee : MonoBehaviour
{
    [Inject] private Input _input;

    public float speedRotate = 1f;

    public Transform target;
    public Rigidbody rb;

    private void FixedUpdate()
    {
        MoveSystem();
        RotateSystem();
    }

    private void MoveSystem()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        
        if (_input.PressForwardMove)
            rb.velocity = target.right;
        if (_input.PressBackMove)
            rb.velocity = -target.right;
        if (_input.PressLeftMove)
            rb.velocity = target.forward;
        if (_input.PressRightMove)
            rb.velocity = -target.forward;
    }

    private void RotateSystem()
    {
        target.Rotate(Vector3.up, _input.deltaMovePosition.x * speedRotate, Space.World);

        target.Rotate(Vector3.forward, _input.deltaMovePosition.y * speedRotate);

        if (target.localEulerAngles.z < 90)
        {
            var newZ = math.clamp(target.localEulerAngles.z, 0, 30);
            target.localEulerAngles =
                new Vector3(target.localEulerAngles.x, target.localEulerAngles.y, newZ);
        }

        if (target.localEulerAngles.z > 300)
        {
            var newZ = math.clamp(target.localEulerAngles.z, 330, 360);
            target.localEulerAngles =
                new Vector3(target.localEulerAngles.x, target.localEulerAngles.y, newZ);
        }
    }
}