using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class FollowTransition : EnemyStateTransition
{
    protected override void OnStateEnter()
    {
        Enemy.Player.OutsideTimer.OnExitBubble.On(Transit);
        if (Enemy.Player.Health.Health == 0)
            return;
        if (Enemy.Player.OutsideTimer.InsideBubble == false)
            Transit();
    }

    protected override void OnStateExited()
    {
        Enemy.Player.OutsideTimer.OnExitBubble.Off(Transit);
    }

    
}
