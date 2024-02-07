using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;

public class DeathFx : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private Transform _model;
    [SerializeField] private AnimatorLinker _animatorLinker;
    [SerializeField, ShowIf(nameof(HaveAnimator)), AnimatorParam(nameof(Animator))]
    private string _deathParameter;
    [SerializeField] private ParticleSystem _deathParticle;
    [SerializeField] private float _removeDelay;

    private bool HaveAnimator => _animatorLinker != null && _animatorLinker.Animator != null;
    private Animator Animator => _animatorLinker.Animator;

    private void OnEnable()
    {
        _health.Died += OnDie;
    }

    private void OnDisable()
    {
        _health.Died -= OnDie;
    }

    private void OnDie()
    {
        Animator.SetTrigger(_deathParameter);
        DOVirtual.DelayedCall(_removeDelay, () =>
        {
            _deathParticle.Play();
            _model.gameObject.SetActive(false);
        });
    }
}
