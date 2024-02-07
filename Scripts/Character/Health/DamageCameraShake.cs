using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;

public class DamageCameraShake : MonoBehaviour
{
    [SerializeField] private CameraShake _cameraShake;
    [SerializeField] private Health _health;

    private void OnEnable()
    {
        _health.Damaged += OnDamaged;
    }
    
    private void OnDisable()
    {
        _health.Damaged += OnDamaged;
    }

    private void OnDamaged(int damage)
    {
        _cameraShake.Shake();
    }

}
