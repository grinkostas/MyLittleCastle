using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamageAnimation : MonoBehaviour
{
    [SerializeField] private Destructible _destructible;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _animationTrigger;

    private void OnEnable()
    {
        _destructible.Damaged += OnDamaged;
    }

    private void OnDisable()
    {
        _destructible.Damaged -= OnDamaged;
    }
    
    private void OnDamaged(int damage)
    {
        _animator.SetTrigger(_animationTrigger);
    }
}
