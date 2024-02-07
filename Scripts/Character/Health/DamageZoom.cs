using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class DamageZoom : MonoBehaviour
{
    [SerializeField] private Destructible _destructible;
    [SerializeField] private Transform _zoomTarget;
    [SerializeField] private float _zoomDuration;
    [SerializeField] private Vector3 _zoomPunch;

    private Tweener _zoomTweener;
    
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
        if(_zoomTweener is {active: true})
            return;
        _zoomTweener = _zoomTarget.DOPunchScale(_zoomPunch, _zoomDuration, 2);
    }
}
