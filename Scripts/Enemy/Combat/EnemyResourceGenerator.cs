using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using DG.Tweening;
using JetBrains.Annotations;
using StaserSDK.Stack;
using Zenject;

public class EnemyResourceGenerator : ResourceGenerator
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private Health _health;
    [SerializeField] private List<ItemType> _possibleResources;
    [SerializeField] private float _spawnDelay;

    public override Transform Parent => ResourceController.DefaultParent;
    
    private void OnEnable()
    {
        _health.Died += OnDie;
    }

    private void OnDisable()
    {
        _health.Died -= OnDie;
    }

    private void OnDie()
    {
        DOVirtual.DelayedCall(_spawnDelay, () => SpawnResource(_possibleResources.Random()));
    }

    protected override void SetStartPosition(StackItem stackItem)
    {
        stackItem.transform.position = _enemy.transform.position;
    }

}
