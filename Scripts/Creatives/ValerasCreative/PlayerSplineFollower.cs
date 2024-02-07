using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Dreamteck.Splines;
using NaughtyAttributes;
using NepixSignals;

public class PlayerSplineFollower : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private SplinePositioner _splinePositioner;
    [SerializeField] private RotationSmoother _rotationSmoother;
    [SerializeField] private float _speed;
    [SerializeField] private float _speedRatio;
    [SerializeField] private float _movementEnableDelay;
    [SerializeField] private float _startPercent;
    [SerializeField] private float _endPercent;

    private const string SpeedRatio = "Speed";
    public TheSignal EndedPath { get; } = new TheSignal();
    
    [Button]
    public void Follow()
    {
        if(DOTween.IsTweening(this))
            return;
        SetPercent(0);
        SetTarget();
        StartMove();
    }

    [Button()]
    private void Skip()
    {
        EndedPath.Dispatch();
    }

    private void SetPercent(float percent)
    {
        var length = _splinePositioner.CalculateLength();
        _splinePositioner.SetDistance(length * percent);
    }

    private void SetTarget()
    {
        _player.Movement.gameObject.SetActive(false);
        _splinePositioner.targetObject = _player.gameObject;
    }

    private void StartMove()
    {
        _rotationSmoother.Enable(_splinePositioner);
        var length = _splinePositioner.CalculateLength();
        var moveTime = length / _speed;
        _player.Animator.SetFloat(SpeedRatio, _speedRatio);
        DOVirtual.Float(_startPercent, _endPercent, moveTime, SetPercent)
            .SetEase(Ease.Linear)
            .SetId(this)
            .OnComplete(EndMove);
    }

    private void EndMove()
    {
        _rotationSmoother.Disable();
        _splinePositioner.targetObject = null;
        _player.Animator.SetFloat(SpeedRatio, 0);
        EndedPath.Dispatch();
    }
}
