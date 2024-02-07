using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;

public class DisabledWeaponShakeEffect : MonoBehaviour
{
    [SerializeField] private WeaponController _weaponController;
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private Vector3 _punch;
    [SerializeField] private float _duration;
    [SerializeField] private int _vibrato;
    [SerializeField] private float _randomness;

    private Tweener _shakeTweener;
    
    private void OnEnable()
    {
        _weaponController.UsedDisabledWeapon += Shake;
    }

    private void OnDisable()
    {
        _weaponController.UsedDisabledWeapon -= Shake;
    }

    [Button("Shake")]
    private void Shake()
    {
        if(_shakeTweener is {active: true})
            return;
        _shakeTweener = _targetTransform.DOShakePosition(_duration, _punch, _vibrato, _randomness);
    }
    
}
