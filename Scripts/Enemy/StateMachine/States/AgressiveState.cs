using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Zenject;

public class AgressiveState : EnemyState
{
    [SerializeField] private float _speedModifier;
    [SerializeField] private NumericAction _speedModifierAction;
    
    protected override void OnEnter()
    {
        Enemy.Movement.AddSpeedModifier(new SpeedModifier(this, _speedModifier, _speedModifierAction));
    }

    protected override void OnExit()
    {
        Enemy.Movement.RemoveSpeedModifier(this);
    }
    
    private void Update()
    {
        Enemy.Movement.SetDestination(Enemy.Player.transform.position);
    }
}
