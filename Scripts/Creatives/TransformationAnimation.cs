using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class TransformationAnimation : MonoBehaviour
{
    [SerializeField] private TransformationFx _transformationFx;
    [SerializeField] private AnimatorLinker _animator;
    [SerializeField] private float _startDelay;
    [SerializeField] private float _endDelay;
    [SerializeField] private string _fallParameter;
    [SerializeField] private string _idleParameter;
    private void OnEnable()
    {
        _transformationFx.Started.On(OnTransformation);
    }

    private void OnDisable()
    {
        _transformationFx.Started.Off(OnTransformation);
    }

    private void OnTransformation()
    {
        DOVirtual.DelayedCall(_startDelay, () => _animator.Animator.SetTrigger(_fallParameter));
        DOVirtual.DelayedCall(_startDelay + _endDelay, () => _animator.Animator.SetTrigger(_idleParameter));
    }
}
