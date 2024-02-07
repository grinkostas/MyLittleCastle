using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class DamagePresenter : MonoBehaviour
{
    [SerializeField] private MonoPool<DamagePoint> _damagePool;
    [SerializeField] private Health _health;

    private void Awake()
    {
        _damagePool.Initialize();
    }

    private void OnEnable()
    {
        _health.Damaged += OnTakeDamage;
    }

    private void OnDisable()
    {
        _health.Damaged -= OnTakeDamage;
    }
    
    private void OnTakeDamage(int damage)
    {
        var damagePoint = _damagePool.Get();
        damagePoint.SetDamage(damage);
    }
}
