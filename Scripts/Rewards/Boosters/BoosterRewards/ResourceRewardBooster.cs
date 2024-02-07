using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using NaughtyAttributes;
using NepixSignals;
using StaserSDK.Interactable;
using StaserSDK.Stack;
using Zenject;

public class ResourceRewardBooster : RewardBooster
{
    [SerializeField] private ItemType _rewardType;
    [SerializeField] private int _rewardCount;
    [SerializeField] private ResourceView _resourceView;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private bool _selfUsage;
    [SerializeField, ShowIf(nameof(_selfUsage))] private BoosterModel _boosterModel;

    [Inject, UsedImplicitly] public ResourceController ResourceController { get; }
    
    private void OnEnable()
    {
        Actualize();
    }

    public void SetReward(ItemType rewardType, int rewardCount)
    {
        _rewardType = rewardType;
        _rewardCount = rewardCount;
        Actualize();
    }

    private void Actualize()
    {
        if(_selfUsage)
            _boosterModel.Init(this);
        _resourceView.Init(_rewardType, _rewardCount);
    }

    protected override void OnTake()
    {
        for (int i = 0; i < _rewardCount; i++)
        {
            DOVirtual.DelayedCall(_spawnDelay * i, () =>
            {
                var item = ResourceController.GetInstance(_rewardType);
                item.Claim();
                item.transform.position = _spawnPoint.position;
                Player.Stack.GetStack(item.Type).Add(item);
            }).SetId(this);
        }
    }
}
