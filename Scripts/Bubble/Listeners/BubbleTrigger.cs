using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NepixSignals;
using Zenject;

[RequireComponent(typeof(Collider))]
public class BubbleTrigger : MonoBehaviour
{
    public TheSignal<BubbleZone> OnEnterBubble { get; } = new();
    public TheSignal<BubbleZone> OnExitBubble { get; } = new();

    private List<BubbleZone> _zones = new();

    public bool InsideBubble { get; private set; }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BubbleZone zone))
        {
            InsideBubble = true;
            if(_zones.Count == 0) 
                OnEnterBubble.Dispatch(zone);
            _zones.Add(zone);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out BubbleZone zone))
        {
            _zones.Remove(zone);
            if(_zones.Count > 0)
                return;
            InsideBubble = false;
            OnExitBubble.Dispatch(zone);
        }
    }
}
