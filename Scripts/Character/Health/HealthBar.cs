using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using DG.Tweening;
using MPUIKIT;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private MPImage _healthBar;
    [SerializeField] private MPImage _tempHealthBar;
    [SerializeField] private float _tempHealthBarFillDuration;
    [SerializeField] private float _tempHealthBarFillDelay;
    [SerializeField] private bool _invert;
    private float CurrentProgress => _health.Health / (float)_health.MaxHealth;
    
    private void OnEnable()
    {
        OnHeathChanged();
        _health.HealthChanged += OnHeathChanged;
    }

    private void OnDisable()
    {
        _health.HealthChanged -= OnHeathChanged;
    }

    private float _delay = 0.0f;
    
    private void OnHeathChanged()
    {
        DOTween.Kill(this);
        var progress = CurrentProgress;
        if (_invert)
            progress = 1 - CurrentProgress;
        _healthBar.fillAmount = progress;
        DOVirtual.Float(_tempHealthBar.fillAmount, progress, _tempHealthBarFillDuration,
            value => _tempHealthBar.fillAmount = value).SetId(this);
    }
}
