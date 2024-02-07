using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using StaserSDK.Stack;
using Zenject;

public class PlayerDeathResourceThrowFx : MonoBehaviour
{
    [SerializeField] private PlayerDeath _playerDeath;
    [SerializeField] private MoveFx _moveFx;
    [SerializeField] private StackItemRotator _itemRotator;
    [SerializeField] private Vector3 _destinationDelta;
    [SerializeField] private Vector2 _throwRange;
    [SerializeField] private float _throwDelay;
    
    [Inject, UsedImplicitly] public Player Player { get; }
    [Inject, UsedImplicitly] public ResourceController ResourceController { get; }

    private void OnEnable()
    {
        _playerDeath.TookItems.On(OnTakeItems);
        _playerDeath.Respawned.On(OnRespawn);
    }

    private void OnDisable()
    {
        _playerDeath.TookItems.Off(OnTakeItems);
    }

    private List<StackItem> _items = new();
    private void OnTakeItems(List<StackItem> items)
    {
        DOVirtual.DelayedCall(_throwDelay, () =>
        {
            foreach (var item in items)
            {
                item.transform.SetParent(ResourceController.DefaultParent, true);
                var delta = Random.insideUnitCircle.XZ() * _throwRange.Random() + _destinationDelta;
                _items.Add(item);
                item.Claim();
                _moveFx.Move(item.transform, ResourceController.DefaultParent, Player.transform.position + delta);
                _itemRotator.Rotate(item.Wrapper, _moveFx.Duration);
                _playerDeath.Collector.Clear();
            }
        });
        
        _playerDeath.Collector.Clear();
    }

    private void OnRespawn()
    {
        foreach (var item in _items)
        {
            item.gameObject.SetActive(false);
            item.Pool.Return(item);
        }
        _items.Clear();
    }
}
