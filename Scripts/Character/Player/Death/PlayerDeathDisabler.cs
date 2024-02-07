using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Utilities;
using Zenject;

public class PlayerDeathDisabler : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private PlayerRespawn _playerRespawn;
    [SerializeField] private WeaponController _weaponController;
    [SerializeField] private List<GameObject> _objectsToDisable;
    [SerializeField] private float _enableDelay;

    [Inject] private Timer _timer;

    private void OnEnable()
    {
        _health.Died += OnDie;
        _playerRespawn.Respawned += OnRespawn;
    }

    private void OnDisable()
    {
        _health.Died -= OnDie;
        _playerRespawn.Respawned -= OnRespawn;
    }


    private void OnDie()
    {
        SetActive(false);
    }

    private void OnRespawn()
    {
        _timer.ExecuteWithDelay(() =>
            SetActive(true), _enableDelay);
    }

    private void SetActive(bool active)
    {
        foreach (var objectToDisable in _objectsToDisable)
            objectToDisable.SetActive(active);

        if (active == false)
            _weaponController.Disable();
        else
            _weaponController.Enable();
    }
}
