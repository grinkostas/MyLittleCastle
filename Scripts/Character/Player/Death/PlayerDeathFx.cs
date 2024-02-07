using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using StaserSDK.Stack;
using StaserSDK.Utilities;
using Zenject;
using Random = UnityEngine.Random;

public class PlayerDeathFx : MonoBehaviour
{

    [SerializeField] private Player _player;
    [Space]
    [SerializeField, ShowIf(nameof(HasAnimator)), AnimatorParam(nameof(Animator))]
    private string _dieParameter;
    [SerializeField, ShowIf(nameof(HasAnimator)), AnimatorParam(nameof(Animator))]
    private string _idleParameter;
    [Space]
    [SerializeField] private Health _health;
    [SerializeField] private PlayerRespawn _playerRespawn;
    [SerializeField] private ParticleSystem _particleSystem;

    [Inject] private Timer _timer;
    
    private bool HasAnimator => _player != null;
    private Animator Animator => _player.Animator;

    private void OnEnable()
    {
        _health.Died += OnDie;
        _playerRespawn.Respawned += OnRespawn;
    }

    private void OnDisable()
    {
        _health.Died -= OnDie;
        _playerRespawn.Respawned -= OnRespawn;
    }


    private void OnDie()
    {
        _particleSystem.Play();
        Animator.SetTrigger(_dieParameter);
    }

    private void OnRespawn()
    {
        Animator.SetTrigger(_idleParameter);
    }

}
