using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NepixSignals;
using StaserSDK.Utilities;
using Zenject;

public class BuyZoneActivator : ABuyZoneActivator
{
    [SerializeField] private BuyZone _buyZone;
    [SerializeField] private List<GameObject> _activateObjects;
    [SerializeField] private List<GameObject> _disableObject;
    [SerializeField] private float _buyZoneZoomOutDuration;
    [SerializeField] private float _enableDelay;
    
    public override TheSignal Bought => _buyZone.Bought;

    public override bool IsBought() => _buyZone.IsBoughtCheck();

    private void OnEnable()
    {
        DisableAll();
        _buyZone.Bought.On(EnableAll);
    }

    private void OnDisable()
    {
        _buyZone.Bought.Off(EnableAll);
    }

    public void EnableAll()
    {
        var zoomOutTweener = _buyZone.transform.DOScale(Vector3.zero, _buyZoneZoomOutDuration);
        
        foreach (var disableObject in _disableObject)
            disableObject.transform.DOScale(new Vector3(0, 0, 0), _buyZoneZoomOutDuration).SetEase(Ease.InBack)
                .OnComplete(() => disableObject.SetActive(false));
        
        foreach (var objectToActivate in _activateObjects)
            DOVirtual.DelayedCall(_enableDelay, () => objectToActivate.SetActive(true));

        if (zoomOutTweener.active)
            zoomOutTweener.OnComplete(() => _buyZone.gameObject.SetActive(false));
        else
            _buyZone.gameObject.SetActive(false);
    }

    public override void DisableAll(bool disableZone = false)
    {
        foreach (var activateObject in _activateObjects)
            activateObject.SetActive(false);
        if(disableZone)
            _buyZone.gameObject.SetActive(false);
        
        
    }
    
}
