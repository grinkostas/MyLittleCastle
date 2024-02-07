using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgressiveTransition : EnemyStateTransition
{
    [SerializeField] private FollowState _followState;
    [SerializeField] private float _maxAgrDistance = 5.0f;
    [SerializeField] private Destructible _destructible;
    [SerializeField] private float _attackRange;

    private bool _entered = false;
    
    protected override void OnStateEnter()
    {
        _entered = true;
        if(Enemy.Player.Health.Health == 0)
            return;

        if (VectorExtentions.SqrDistance(Enemy.transform, Enemy.Player.transform) > _maxAgrDistance)
        {
            return;
        }
        
        if (Enemy.Player.OutsideTimer.Progress >= 0.95f)
        {
            Transit();
            return;
        }

        _destructible.Damaged += OnDamaged;
        Enemy.Player.OutsideTimer.ProgressChanged += OnTimerProgressChanged;

    }

    protected override void OnStateExited()
    {
        _entered = false;
        _destructible.Damaged -= OnDamaged;
        Enemy.Player.OutsideTimer.ProgressChanged -= OnTimerProgressChanged;
    }

    private void OnTimerProgressChanged(float progress)
    {
        if(progress >= 0.95f)
            Transit();
    }

    private void OnDamaged(int damage)
    {
        _destructible.Damaged -= OnDamaged;
        Transit();
    }

    private void Update()
    {
        if(_entered == false)
            return;
        float distance = VectorExtentions.SqrDistance(Enemy.transform.position, Enemy.TargetPostion);
        if(distance <= _attackRange * _attackRange)
            Transit();
    }

}
