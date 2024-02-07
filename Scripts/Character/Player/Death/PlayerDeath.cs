using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NepixSignals;
using StaserSDK.Stack;
using StaserSDK.Utilities;
using UnityEngine.Polybrush;
using Zenject;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private Collector _collector;
    
    [Inject] private Player _player;
    [Inject] private Timer _timer;

    public Collector Collector => _collector;
    public List<StackItem> LostItems { get; private set; } = new();

    public TheSignal Died { get; } = new();
    public TheSignal Respawned { get; } = new();
    public TheSignal<List<StackItem>> TookItems { get; } = new();

    private void OnEnable()
    {
        _health.Died += OnDied;
        _health.Respawned.On(OnRespawn);
    }

    private void OnDisable()
    {
        _health.Died -= OnDied;
        _health.Respawned.Off(OnRespawn);
    }
    
    private void OnDied()
    {
        _player.Movement.DisableHandle(this);
        _player.Equipment.gameObject.SetActive(false);
        _player.Stack.MainStack.DisableCollect(this);
        _player.Equipment.WeaponController.Disable();
        LostItems = new List<StackItem>(_player.Stack.MainStack.SourceItems);
        _collector.Clear();
        _player.Stack.MainStack.Clear();
        TookItems.Dispatch(LostItems);
        Died.Dispatch();
    }

    private void OnRespawn()
    {
        _player.Movement.EnableHandle(this);
        _player.Equipment.gameObject.SetActive(true);
        _player.Stack.MainStack.EnableCollect(this);
        _player.Equipment.WeaponController.Enable();
        Respawned.Dispatch();
    }

}
