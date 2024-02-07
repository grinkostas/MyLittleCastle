using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BaseBubbleListener : MonoBehaviour
{
    public abstract BubbleTrigger BubbleTrigger { get; }
    public bool Inside;
    
    private List<BubbleZone> _zones = new();
    
    private void OnEnable()
    {
        BubbleTrigger.OnEnterBubble.On(OnEnterBubble);
        BubbleTrigger.OnExitBubble.On(OnExitBubble);
        OnEnableInternal();
    }
    
    private void OnDisable()
    {
        BubbleTrigger.OnEnterBubble.Off(OnEnterBubble);
        BubbleTrigger.OnExitBubble.Off(OnExitBubble);
        OnDisableInternal();
    }

    private void OnEnterBubble(BubbleZone zone)
    {
        Inside = true;
        _zones.Add(zone);
        OnInsideBubble();
    }

    private void OnExitBubble(BubbleZone zone)
    {
        _zones.Remove(zone);
        if(_zones.Count > 0)
            return;
        Inside = false;
        OnOutsideBubble();
    }

    protected virtual void OnInsideBubble()
    {
    }

    protected virtual void OnOutsideBubble()
    {
    }

    protected virtual void OnEnableInternal()
    {
    }
    
    protected virtual void OnDisableInternal()
    {
    }
}
