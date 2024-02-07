using System;
using DG.Tweening;
using NepixSignals;
using StaserSDK.Interactable;
using UnityEngine;


public class EnterZoneRewardActivator : MonoBehaviour
{
    [SerializeField] private RewardBooster _reward;
    [SerializeField] private float _takeDelay;
    [SerializeField] private float _hideDelay;
    [SerializeField] private Transform _zoomTarget;
    [SerializeField] private float _zoomDuration;
    [SerializeField] private ZoneBase _zoneBase;

    public TheSignal Activated { get; } = new();
    
    private void OnEnable()
    {
        _zoneBase.OnInteract += OnInteract;
    }

    private void OnDisable()
    {
        _zoneBase.OnInteract -= OnInteract;
    }

    private void OnInteract(InteractableCharacter interactableCharacter)
    {
        Activated.Dispatch();
        _zoneBase.OnInteract -= OnInteract;
        DOTween.Kill(this);
        DOVirtual.DelayedCall(_takeDelay, () =>
        {
            _reward.Take();
            _zoomTarget.DOScale(0, _zoomDuration).SetEase(Ease.InBack).SetDelay(_hideDelay).SetId(this);
        }).SetId(this);
    }
}
