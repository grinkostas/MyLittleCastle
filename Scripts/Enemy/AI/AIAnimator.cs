using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;

public class AIAnimator : MonoBehaviour
{
    [SerializeField] protected AIMovement _aiMovement;
    [SerializeField] protected Animator Animator;

    [SerializeField, ShowIf(nameof(HasAnimator)), AnimatorParam(nameof(Animator))]
    private string _walkingParamRatio;

    [SerializeField] private float _minMoveRatio;
    [SerializeField] private float _maxSpeed;

    [SerializeField] private bool _animateRotation;
    
    [SerializeField, ShowIf(nameof(_animateRotation))] 
    private float _minDot;

    [SerializeField, ShowIf(nameof(_animateRotation))]
    private float _maxDot;
    
    [SerializeField, ShowIf(nameof(_animateRotation))] 
    private int _rotateLayerIndex;
    
    [SerializeField, ShowIf(nameof(_animateRotation)), AnimatorParam(nameof(Animator))]
    private string _turnLeftParameter;
    
    [SerializeField, ShowIf(nameof(_animateRotation)), AnimatorParam(nameof(Animator))]
    private string _turnRightParameter;
    
    [SerializeField, ShowIf(nameof(_animateRotation)), AnimatorParam(nameof(Animator))]
    private string _stopRotationParameter;
    protected bool HasAnimator => Animator != null;
    

    private void OnEnable()
    {
        _aiMovement.OnStartMove += OnStartMove;
        _aiMovement.OnMove += OnMove;
        if(_animateRotation)
            _aiMovement.Rotated += OnRotate;
        _aiMovement.OnStopMove += StopMove;
    }

    private void OnDisable()
    {
        _aiMovement.OnStartMove -= OnStartMove;
        _aiMovement.OnMove -= OnMove;
        if(_animateRotation)
            _aiMovement.Rotated -= OnRotate;
        _aiMovement.OnStopMove -= StopMove;
    }

    protected virtual void OnMove(Vector3 input)
    {
        if(_aiMovement.enabled == false)
            return;
        ChangeSpeedRatio();
    }

    protected virtual void OnStartMove()
    {
        if(_aiMovement.enabled == false)
            return;
        ChangeSpeedRatio();
    }
    
    private void ChangeSpeedRatio()
    {
        if (_aiMovement.Speed <= 0.05f)
        {
            StopMove();
            return;
        }
        float moveRatio = Mathf.Clamp(_aiMovement.Speed / _maxSpeed, _minMoveRatio, 1);
        
        Animator.SetFloat(_walkingParamRatio, moveRatio);
    }

    private void StopMove()
    {
        if(_aiMovement.enabled == false)
            return;
        Animator.SetFloat(_walkingParamRatio, 0);
    }

    private void TurnLeft() => Turn(true);
    private void TurnRight() => Turn(false, true);
    private void StopRotate() => Turn(false, false, true);
    
    private void Turn(bool left, bool right = false, bool stop = false)
    {
        Animator.SetBool(_turnLeftParameter, left);
        Animator.SetBool(_turnRightParameter, right);
        if(stop)
            Animator.SetTrigger(_stopRotationParameter);
    }

    private void OnRotate(Vector3 angle)
    {
        float yAngle = angle.y;
        float dot = Mathf.Clamp01(Mathf.Abs(yAngle) / _maxDot);

        if (yAngle <= _minDot)
        {
            StopRotate();
            return;
        }
        
        if (yAngle > 0)
            TurnRight();
        else
            TurnLeft();

        Animator.SetLayerWeight(_rotateLayerIndex, dot);
    }
}
