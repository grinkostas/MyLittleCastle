using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Zenject;
using StaserSDK.Utilities;

public class BubbleHider : MonoBehaviour
{
    [SerializeField] private float _hideDelay;
    [SerializeField] private View _view;
    [SerializeField] private bool _hide;
    [SerializeField] private DestructibleRespawner _respawner;

    [Inject] private Bubble _bubble;

    private void OnEnable()
    {
        _bubble.EndScale += OnEndScale;
    }

    private void OnDisable()
    {
        _bubble.EndScale -= OnEndScale;
    }

    private void OnEndScale()
    {
        if (_hide == false)
        {
            _respawner.RespawnModifier = new FloatModifier(5, NumericAction.Multiply);
            return;
        }

        if (_bubble.IsLocateInside(transform.position))
            DOVirtual.DelayedCall(_hideDelay, _view.Hide);
    }
    
}
