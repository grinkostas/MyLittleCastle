using System.Collections.Generic;
using UnityEngine;

public class BuyZoneGroupBubbleIncreaser : MonoBehaviour
{
    [SerializeField] private Bubble _bubble;
    [SerializeField] private List<InternalData> _data;
    
    [System.Serializable]
    public class InternalData
    {
        public BuyZoneEvents Events;
        public float TargetScale;
    }
    
    private void OnEnable()
    {
        ActualizeSize();
        foreach (var data in _data)
            data.Events.Bought.On(ActualizeSize);
        
    }

    private void OnDisable()
    {
        foreach (var data in _data)
            data.Events.Bought .Off(ActualizeSize);
    }

    private void ActualizeSize()
    {
        float bubbleSize = _bubble.StartSize;
        foreach (var data in _data)
        {
            if(data.Events.Zone.IsBought() == false)
                continue;
            if(data.TargetScale < bubbleSize)
                continue;
            bubbleSize = data.TargetScale;
        }
        if(bubbleSize > _bubble.StartSize)
            SetSize(bubbleSize);
    }

    private void SetSize(float size)
    {
        _bubble.Increase(size, showCamera:false);
    }
}
