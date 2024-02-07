using System;
using System.Collections.Generic;
using UnityEngine;


public abstract class BubbleListener : BaseBubbleListener
{
    [SerializeField] private BubbleTrigger _bubbleTrigger;
    public override BubbleTrigger BubbleTrigger => _bubbleTrigger;
}
