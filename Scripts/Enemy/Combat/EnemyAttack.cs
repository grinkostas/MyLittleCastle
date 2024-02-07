using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using NaughtyAttributes;
using NepixSignals;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private int _hitDamage;
    [SerializeField] private float _targetAttackRange;
    [SerializeField] private float _maxAttackRange;

    public float TargetAttackRange => _targetAttackRange;
    public float MaxAttackRange => _maxAttackRange;
    public Enemy Enemy => _enemy;
    public TheSignal AttackCompleted { get; } = new();
    public TheSignal AttackEnded { get; } = new();

    [UsedImplicitly]
    public void ApplyAttack()
    {
        AttackCompleted.Dispatch();
        if(VectorExtentions.SqrDistance(_enemy.transform.position, _enemy.TargetPostion) > Mathf.Pow(_maxAttackRange, 2))
            return;
        _enemy.Player.Health.ApplyDamage(_hitDamage);
    }
    
    [UsedImplicitly]
    public void EndAttack()
    {
        AttackEnded.Dispatch();
    }
}
