using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;

public class ForwardMover : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private float _speed;
    
    private bool _isMoving = false;
    private void Update()
    {
        if(_isMoving == false)
            return;
        _target.position += _target.forward * (_speed * Time.deltaTime);
    }

    [Button]
    public void StartMover()
    {
        _isMoving = true;
    }

    [Button]
    private void Restart()
    {
        _isMoving = false;
        _target.position = _startPosition;
    }

    public void Stop()
    {
        _isMoving = false;
    }
}
