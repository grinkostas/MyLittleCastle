using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetDistanceTransition : EnemyStateTransition
{
    [SerializeField] private float _distanceToTarget;
    [SerializeField] private Comparison _comparison;
    public float SqrDistance;
    protected override void OnStateEnter()
    {
    }

    protected override void OnStateExited()
    {
    }

    private void Update()
    {
        if(Entered == false)
            return;
        
        if(Enemy.Player.OutsideTimer.InsideBubble)
            return;
        
        SqrDistance = VectorExtentions.SqrDistance(transform.position, Enemy.TargetPostion);
        if (_comparison.Compare(SqrDistance, _distanceToTarget*_distanceToTarget))
            Transit();
    }
}
