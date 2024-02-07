using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TorchRefillPresenter : MonoBehaviour
{
    [SerializeField] private View _refillView;
    [SerializeField] private OutsideTimer _outsideTimer;

    private bool _isViewHidden = true;
    
    private void OnEnable()
    {
        Actualize();
        _outsideTimer.ProgressChanged += OnProgressChanged;
        _outsideTimer.BubbleTrigger.OnEnterBubble.On(OnEnterBubble);
    }

    private void OnDisable()
    {
        Actualize();
        _outsideTimer.ProgressChanged -= OnProgressChanged;
        _outsideTimer.BubbleTrigger.OnEnterBubble.Off(OnEnterBubble);
    }

    private void OnProgressChanged(float progress)
    {
        Actualize();
    }

    private void OnEnterBubble(BubbleZone _)
    {
        Actualize();
    }

    private void Actualize()
    {
        if (_outsideTimer.Inside)
        {
            Hide();
            return;
        }

        if (_outsideTimer.Progress > 0.99f)
            Show();
        else
            Hide();
    }

    private void Hide()
    {
        if(_isViewHidden == false)
            _refillView.Hide();
        _isViewHidden = true;
    }

    private void Show()
    {
        if(_isViewHidden)
            _refillView.Show();
        _isViewHidden = false;

    }
}
