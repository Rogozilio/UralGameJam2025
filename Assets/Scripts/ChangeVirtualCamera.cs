using System;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class ChangeVirtualCamera : MonoBehaviour
{
    public CinemachineVirtualCamera camera;

    public Vector3 bodyFollowOffset;
    public float step;

    private CinemachineTransposer _transposer;
    private bool _isActiveChange;

    private float _kof;

    public bool IsActiveChange
    {
        set => _isActiveChange = value;
    }

    private void Awake()
    {
        _transposer = camera.GetCinemachineComponent<CinemachineTransposer>();
    }

    private void FixedUpdate()
    {
        if (!_isActiveChange)
        {
            _kof = 0;
            return;
        }

        _kof += step;
        _transposer.m_FollowOffset = math.lerp(_transposer.m_FollowOffset, bodyFollowOffset, _kof);

        if (_kof > 1f) _isActiveChange = false;
    }
}