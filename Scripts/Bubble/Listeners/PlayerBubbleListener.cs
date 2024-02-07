using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class PlayerBubbleListener : BaseBubbleListener
{
    [Inject] public Player Player { get; }

    public override BubbleTrigger BubbleTrigger => Player.Trigger;
}
