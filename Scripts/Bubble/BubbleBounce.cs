using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using NaughtyAttributes;

public class BubbleBounce : MonoBehaviour
{
    [SerializeField] private bool _writeValues;
    [SerializeField, ShowIf(nameof(_writeValues))] private float _bounceDuration;

    [SerializeField, ShowIf(nameof(_writeValues))] private Vector3 _punch;

    private static Tweener _bounceTweener;
    private static Vector3 _targetScale = Vector3.one;
    
    public static BubbleBounce Instance;
    
    private void OnEnable()
    {
        if(_bounceTweener != null && _writeValues)
            return;
        _bounceTweener = DOTween.Punch(() => _targetScale, x => _targetScale = x, _punch, _bounceDuration, 1, 1).SetLoops(-1);
    }
    
    private void Update()
    {
        transform.localScale = _targetScale;
    }
}
