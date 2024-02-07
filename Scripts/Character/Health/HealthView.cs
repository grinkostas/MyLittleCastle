using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class HealthView : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private int _multiplayer;
    [SerializeField] private TMP_Text _text;

    private void OnEnable()
    {
        OnHealthChanged();
        _health.HealthChanged += OnHealthChanged;
    }
    
    private void OnDisable()
    {
        _health.HealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged()
    {
        _text.text = (_health.Health * _multiplayer).ToString();
    }
}
