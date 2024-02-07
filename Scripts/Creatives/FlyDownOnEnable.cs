using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class FlyDownOnEnable : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private AnimatorLinker _animatorLinker;
    [SerializeField] private float _delay;
    [SerializeField] private float _speed;
    [SerializeField] private float _distanceToIdle;
    
    private static readonly int Landing = Animator.StringToHash("Landing");
    private static readonly int Idle = Animator.StringToHash("Idle");

    
    private bool _fall = false;
    
    private void OnEnable()
    {
        float targetY = _target.transform.position.y;
        _fall = Mathf.Abs(targetY) > 0.5f;
        if(_fall == false)
            return;
        _animatorLinker.Animator.SetTrigger(Landing);
        _target.DOMoveY(0, targetY / _speed).SetDelay(_delay).SetEase(Ease.InQuad);
        DOVirtual.DelayedCall(_delay, _particle.Play).SetId(this);
    }

    private void OnDisable()
    {
        DOTween.Kill(this);
    }

    
    private void Update()
    {
        if(_fall == false)
            return;
        if(_target.transform.position.y > _distanceToIdle)
            return;
        _fall = false;
        DOTween.Kill(this);
        _animatorLinker.Animator.SetTrigger(Idle);
        _particle.Stop();
        _particle.gameObject.SetActive(false);
    }
}
