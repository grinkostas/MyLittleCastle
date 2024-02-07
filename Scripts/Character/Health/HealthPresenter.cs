using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using StaserSDK.Utilities;
using Zenject;

public class HealthPresenter : MonoBehaviour
{
    [SerializeField] private View _view;
    [SerializeField] private float _hideDelay;
    [SerializeField] private Health _health;
    
    private void OnEnable()
    {
        _health.Damaged += OnDamaged;
    }

    private void OnDisable()
    {
        _health.Damaged -= OnDamaged;
    }

    private void OnDamaged(int damage)
    {
        if (_health.Health <= 0)
        {
            _view.Hide();
            return;
        }
        _view.Show();
        DOTween.Kill(this);
        DOVirtual.DelayedCall(_hideDelay, _view.Hide);
    }
}
