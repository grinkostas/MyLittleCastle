using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using StaserSDK.Utilities;
using Zenject;

public class AttackState : EnemyState
{
    [SerializeField, ShowIf(nameof(HaveAnimator)), AnimatorParam(nameof(Animator))] 
    private string _attackParameter;

    protected override void OnEnter()
    {
        Enemy.Movement.AddSpeedModifier(new SpeedModifier(this, 0));
        Enemy.Animator.SetBool(_attackParameter, true);
    }

    protected override void OnExit()
    {
        Enemy.Movement.RemoveSpeedModifier(this);
        Enemy.Animator.SetBool(_attackParameter, false);
    }

}
