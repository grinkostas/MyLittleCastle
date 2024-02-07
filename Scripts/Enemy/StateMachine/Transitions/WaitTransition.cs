using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Zenject;

public class WaitTransition : EnemyStateTransition
{
    [SerializeField] private BubbleTrigger _bubbleTrigger;
    protected override void OnStateEnter()
    {
        if (Enemy.Player.OutsideTimer.InsideBubble)
            Transit();

        _bubbleTrigger.OnEnterBubble.On(OnEnterBubble);
        Enemy.Player.OutsideTimer.OnEnterBubble.On(Transit);
        Enemy.Player.Health.Died += Transit;
        Enemy.Health.Died += Transit;
    }

    protected override void OnStateExited()
    {
        _bubbleTrigger.OnEnterBubble.Off(OnEnterBubble);
        Enemy.Player.OutsideTimer.OnEnterBubble.Off(Transit);
        Enemy.Health.Died -= Transit;
        Enemy.Player.Health.Died -= Transit;
    }

    private void OnEnterBubble(BubbleZone bubbleZone)
    {
        Transit();
    }

}
