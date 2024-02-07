using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackTransition : EnemyStateTransition
{
    [SerializeField] private EnemyAttack _enemyAttack;
    protected override void OnStateEnter()
    {
    }

    protected override void OnStateExited()
    {
    }

    private void Update()
    {
        if(Entered == false || _enemyAttack.Enemy.Player.Health.Health == 0)
            return;
        
        if(VectorExtentions.SqrDistance(Enemy.transform.position, Enemy.TargetPostion) < Mathf.Pow(_enemyAttack.TargetAttackRange, 2))
            Transit();
    }
}
