using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Stack;

public class DestructiblePresetner : MonoBehaviour
{
    [SerializeField] private Destructible _destructible;
    [SerializeField] private View _aliveView;
    [SerializeField] private View _destroyedView;

    private void OnEnable()
    {
        StackItem a;
        _destructible.HealthChanged += OnHealthChange;
    }
    
    private void OnDisable()
    {
        _destructible.HealthChanged -= OnHealthChange;
    }
    
    private void OnHealthChange()
    {
        if (_destructible.Health > 0)
        {
            _aliveView.Show();
            _destroyedView.Hide();
            return;
        }
        
        _destroyedView.Show();
        _aliveView.Hide();
    }
}
