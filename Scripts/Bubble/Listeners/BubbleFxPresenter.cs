using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using StaserSDK.Utilities;
using Zenject;

public class BubbleFxPresenter : MonoBehaviour
{
    [SerializeField] private View _view;
    [SerializeField] private float _showDelay;
    
    [Inject] private Bubble _bubble;

    private void OnEnable()
    {
        _bubble.StartScaling += OnStartScale;
        _bubble.EndScale += OnEndScale;
    }

    private void OnStartScale(float endValue)
    {
        _view.Hide();
    }

    private void OnEndScale()
    {
        DOVirtual.DelayedCall(_showDelay, _view.Show);
    }
}
