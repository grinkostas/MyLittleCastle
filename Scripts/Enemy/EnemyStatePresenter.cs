using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyStatePresenter : MonoBehaviour
{
    [SerializeField] private View _view;
    [SerializeField] private List<EnemyState> _showStates;
    [SerializeField] private List<EnemyState> _hideStates;

    private void OnEnable()
    {
        foreach (var state in _showStates)
            state.Entered += _view.Show;
        
        foreach (var state in _hideStates)
            state.Entered += _view.Hide;
    }

    private void OnDisable()
    {
        foreach (var state in _showStates)
            state.Entered += _view.Show;
        
        foreach (var state in _hideStates)
            state.Entered += _view.Hide;
    }
}
