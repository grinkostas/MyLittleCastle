using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Zenject;

public class EnemyTutorialState : TutorialStepBase
{
    [SerializeField] private GameObject _enemyParent;
    [SerializeField] private float _enemyDisableDelay;
    [SerializeField] private EnemyTutorial _enemyTutorial;
    [SerializeField] private Transform _enemySpawnPoint;
    [SerializeField] private EnemyKillIEvent _enemyKillIEvent;
    
    [Inject] public DiContainer DiContainer { get; }
    
    private Transform _target;
    public override Transform Target => _target;

    private void OnEnable()
    {
        _target = _enemySpawnPoint;
    }

    protected override void OnEnter()
    {
        _enemyParent.SetActive(true);
        _target = _enemyTutorial.PointerParent;
    }

    protected override void OnExit()
    {
        DOVirtual.DelayedCall(_enemyDisableDelay, () => _enemyParent.SetActive(false));
        if(_target == null)
            return;
        _target.gameObject.SetActive(false);
    }
    
    
}
