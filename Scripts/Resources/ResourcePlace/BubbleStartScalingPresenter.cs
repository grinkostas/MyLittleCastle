using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using StaserSDK.Utilities;
using Zenject;

public class BubbleStartScalingPresenter : MonoBehaviour
{
    [SerializeField] private View _view;
    [SerializeField] private float _returnTime;
    [Inject] private Bubble _bubble;

    private void OnEnable()
    {
        
        _bubble.StartScaling += OnStartScaling;
    }
    
    private void OnDisable()
    {
        _bubble.StartScaling -= OnStartScaling;
    }

    private void OnStartScaling(float targetScale)
    {
        _view.Hide();
        DOVirtual.DelayedCall(_returnTime, _view.Show).SetId(this);
    }

    private void OnDestroy()
    {
        DOTween.Kill(this);
    }
}
