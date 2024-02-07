using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using StaserSDK;
using StaserSDK.Utilities;
using UnityEngine.Events;
using Zenject;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private MovementHandlerProvider _movementHandlerProvider;
    [SerializeField] private Transform _respawnPoint;

    [Inject] private Player _player;

    public event UnityAction Respawned;

    private void OnEnable()
    {
        _health.Died += OnDied;
    }

    private void OnDisable()
    {
        _health.Died -= OnDied;
    }

    private void OnDied()
    {
        _health.enabled = false;
        _movementHandlerProvider.Interface.DisableHandle(this);
    }

    public void Respawn()
    {
        _player.transform.position = _respawnPoint.position;
        _movementHandlerProvider.Interface.EnableHandle(this);
        _health.Respawn();
        _health.enabled = true;
        Respawned?.Invoke();
    }
}
