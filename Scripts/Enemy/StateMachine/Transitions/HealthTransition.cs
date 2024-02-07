using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealthTransition : EnemyStateTransition
{
    [SerializeField] private Destructible _health;
    [SerializeField] private HealthAction _healthAction;
    internal enum HealthAction
    {
        Die, 
        Respawn
    }
    
    protected override void OnStateEnter()
    {
        _health.HealthChanged += OnHealthChanged;
    }

    protected override void OnStateExited()
    {
        _health.HealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged()
    {
        if (_healthAction == HealthAction.Die && _health.Health == 0)
            Transit();

        if (_healthAction == HealthAction.Respawn && _health.Health == _health.MaxHealth)
            Transit();
    }
}
