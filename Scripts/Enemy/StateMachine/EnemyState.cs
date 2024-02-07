using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine.Events;

public abstract class EnemyState : MonoBehaviour
{
    [SerializeField] private bool _firstState;
    [SerializeField] protected Enemy Enemy;
    [SerializeField, ShowIf(nameof(HaveAnimator)), AnimatorParam(nameof(Animator))] 
    protected string IdleParameter;
    protected Animator Animator => Enemy.Animator;
    protected bool HaveAnimator => !(Enemy == null || Animator == null);
    public Enemy StateEnemy => Enemy;
    
    public UnityAction Entered { get; set; }
    public UnityAction Exited { get; set; }

    private void Awake()
    {
        if(_firstState)
            Enter();
        else
            Exit();
    }

    private void OnEnable()
    {
        if(_firstState)
            return;
        Enemy.DefaultState.Entered += Exit;
    }

    private void OnDisable()
    {
        if(_firstState)
            return;
        Enemy.DefaultState.Entered -= Exit;
    }

    public void Enter()
    {
        enabled = true;
        Entered?.Invoke();
        OnEnter();
    }

    public void Exit()
    {
        enabled = false;
        Exited?.Invoke();
        OnExit();
    }

    protected abstract void OnEnter();
    protected abstract void OnExit();
}
