using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using StaserSDK.Utilities;
using Zenject;

public class DeathState : EnemyState
{
    [SerializeField] private float _respawnTime;
    [SerializeField] private Health _health;

    [Inject] private SpotManager _spotManager;

    protected override void OnEnter()
    {
        Enemy.Movement.AddSpeedModifier(new SpeedModifier(this, 0));
        DOVirtual.DelayedCall(_respawnTime, Respawn);
    }

    protected override void OnExit()
    {
        Enemy.Movement.RemoveSpeedModifier(this);
        Animator.SetTrigger(IdleParameter);
    }

 
    private void Respawn()
    {
        _health.Respawn();
        var spotToRespawn = _spotManager.GetSpot(Enemy);
        if(spotToRespawn == null)
            return;
        Enemy.transform.position = spotToRespawn.transform.position;
    }
}
