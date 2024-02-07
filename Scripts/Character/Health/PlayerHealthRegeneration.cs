using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using Zenject;

public class PlayerHealthRegeneration : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private float _startHealingDelay;
    [SerializeField] private int _healthRegenAmount;
    [SerializeField] private float _healthRegenDela;
    
    [Inject, UsedImplicitly] public Player Player { get; }

    private void OnEnable()
    {
        Player.OutsideTimer.OnEnterBubble.On(OnEnterBubble);
        Player.OutsideTimer.OnExitBubble.On(OnExitBubble);
    }

    private void OnEnterBubble()
    {
        DOVirtual.DelayedCall(_startHealingDelay, Heal).SetId(this);
    }

    private void Heal()
    {
        _health.Heal(_healthRegenAmount);
        if(_health.Health == _health.MaxHealth)
            return;
        DOVirtual.DelayedCall(_healthRegenDela, Heal).SetId(this);
    }
    
    private void OnExitBubble()
    {
        DOTween.Kill(this);
    }
}
