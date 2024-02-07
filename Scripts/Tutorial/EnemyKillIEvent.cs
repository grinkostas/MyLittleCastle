using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class EnemyKillIEvent : MonoBehaviour, ITutorialEvent
{
    [SerializeField] private Enemy _enemy;
    
    public UnityAction Finished { get; set; }
    public UnityAction Available { get; set; }
    public UnityAction<float> ProgressChanged { get; set; }
    public float Progress =>  1 - (_enemy.Health.Health / (float)_enemy.Health.MaxHealth);
    public float FinalValue => 1;

 
    private void OnEnable()
    {
        _enemy.Health.HealthChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        _enemy.Health.HealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged()
    {
        ProgressChanged?.Invoke(Progress);
        if(_enemy.Health.Health == 0)
            Finished?.Invoke();
    }

    public bool IsFinished()
    {
        return _enemy.Health.Health == 0;
    }

    public bool IsAvailable()
    {
        return true;
    }
}
