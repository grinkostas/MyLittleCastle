using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OutsideTimerPresenter : MonoBehaviour
{
    [SerializeField] private OutsideTimer _outsideTimer;
    [SerializeField] private View _view;

    private bool _hidden = true;
    private void OnEnable()
    {
        _outsideTimer.ProgressChanged += OnProgressChanged;
        _outsideTimer.Modifier.Changed.On(OnChange);
    }
    
    private void OnDisable()
    {
        _outsideTimer.ProgressChanged -= OnProgressChanged;
        _outsideTimer.Modifier.Changed.Off(OnChange);
    }

    private void OnChange()
    {
        OnProgressChanged(_outsideTimer.Progress);
    }
    
    private void OnProgressChanged(float progress)
    {
        if (progress > 0.01f)
        {
            if (_hidden)
            {
                _view.Show();
                _hidden = false;
            }
        }
        else
        {
            if (_hidden == false)
            {
                _view.Hide();
                _hidden = true;
            }
        }
    }
}
