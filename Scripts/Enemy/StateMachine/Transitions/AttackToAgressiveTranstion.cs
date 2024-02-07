using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackToAgressiveTranstion : EnemyStateTransition
{
    [SerializeField] private EnemyAttack _enemyAttack;
    
    protected override void OnStateEnter()
    {
        _enemyAttack.AttackCompleted.On(Actualize);
        _enemyAttack.AttackEnded.On(Actualize);
    }

    protected override void OnStateExited()
    {
        _enemyAttack.AttackCompleted.Off(Actualize);       
        _enemyAttack.AttackEnded.Off(Actualize);
    }

    private void Actualize()
    {
         if(VectorExtentions.SqrDistance(Enemy.transform.position, Enemy.TargetPostion) > Mathf.Pow(_enemyAttack.MaxAttackRange, 2))
             Transit();
    }
}
