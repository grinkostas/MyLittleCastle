using System;
using System.Collections.Generic;
using DG.Tweening;
using NepixSignals.Api;
using UnityEngine;
using UnityEngine.Serialization;


public class BuyZonesController : MonoBehaviour
{
    [SerializeField] private List<BuyZoneData> _buyZonesData;

    [System.Serializable]
    public class BuyZoneData
    {
        public ABuyZoneActivator Activator;
        public List<BuyZoneActivator> NextZones;
        public float ActivateDelay;
    }
    
    private List<ISignalCallback> _subscribeCallbacks = new();

    private void OnEnable()
    {
        Actualize();
        Subscribe();
    }

    private void OnDisable()
    {
        foreach (var callback in _subscribeCallbacks)
            callback.Off();
        _subscribeCallbacks.Clear();
    }

    private void Actualize()
    {
        foreach (var buyZoneData in _buyZonesData)
        {
            if (buyZoneData.Activator.IsBought()) 
                continue;
            buyZoneData.Activator.DisableAll();
            buyZoneData.NextZones.ForEach(x=>
            {
                x.DisableAll(true);
            });
        }
    }

    private void Subscribe()
    {
        foreach (var buyZoneData in _buyZonesData)
        {
            var callback = buyZoneData.Activator.Bought.On(() => OnBought(buyZoneData.Activator));
            _subscribeCallbacks.Add(callback);
        }
    }

    private void OnBought(ABuyZoneActivator activator)
    {
        var zoneData = _buyZonesData.Find(x => x.Activator == activator);
        if(zoneData == null)
            return;

        DOVirtual.DelayedCall(zoneData.ActivateDelay, () =>
        {
            zoneData.NextZones.ForEach(x => x.gameObject.SetActive(true));
        });
    }
    
}
