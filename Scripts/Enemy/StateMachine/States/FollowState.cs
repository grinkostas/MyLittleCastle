using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using StaserSDK.Utilities;
using Zenject;

public class FollowState : EnemyState
{
    [SerializeField] private float _distanceToTarget;
    [SerializeField] private float _offset;

    private Vector3 EnemyPosition => Enemy.Movement.transform.position;

    private string _currentAnimation;

    private TimerDelay _sitTimerDelay;
    public bool HaveTarget { get; private set; } = false;
    
    protected override void OnEnter()
    {
        HaveTarget = false;
    }

    protected override void OnExit()
    {
        HaveTarget = false;
        _sitTimerDelay?.Kill();
    }

    private void Update()
    {
        float sqrDistanceToTarget = VectorExtentions.SqrDistance(EnemyPosition, Enemy.TargetPostion);
        if (HaveTarget == false && sqrDistanceToTarget > _distanceToTarget * _distanceToTarget)
        {
            Stop();
            return;
        }
        HaveTarget = true;

        if (sqrDistanceToTarget > Mathf.Pow((_distanceToTarget + _offset), 2))
        {
            Stop();
            HaveTarget = false;
            return;
        }
        Enemy.Movement.SetDestination(Enemy.TargetPostion);
    }

    private void Stop()
    {
        if(enabled == false)
            return;
        if (_currentAnimation == IdleParameter)
        {
            _sitTimerDelay?.Kill();
            Animator.SetTrigger(IdleParameter);
        }

        Enemy.Movement.StopMove();
    }
}
