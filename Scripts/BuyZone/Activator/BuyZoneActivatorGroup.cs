using System;
using System.Collections.Generic;
using NepixSignals;
using Unity.VisualScripting;
using UnityEngine;

public class BuyZoneActivatorGroup : ABuyZoneActivator
{
    [SerializeField] private List<ABuyZoneActivator> _activators;

    public override TheSignal Bought { get; } = new();

    private void OnEnable()
    {
        foreach (var activator in _activators)
            activator.Bought.On(Actualize);
    }
    
    private void OnDisable()
    {
        foreach (var activator in _activators)
            activator.Bought.Off(Actualize);
    }

    private void Actualize()
    {
        if(IsBought())
            Bought.Dispatch();
            
    }
    
    public override void DisableAll(bool disableZone = false)
    {
        foreach (var activator in _activators)
            activator.DisableAll(true);
    }

    public override bool IsBought()
    {
        return _activators.TrueForAll(x => x.IsBought());
    }
}
