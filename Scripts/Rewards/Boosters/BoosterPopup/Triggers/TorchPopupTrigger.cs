using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TorchPopupTrigger : BoosterPopupTrigger
{
    private void OnEnable()
    {
        Player.OutsideTimer.Ended.On(OnEnd);
    }
    
    private void OnDisable()
    {
        Player.OutsideTimer.Ended.Off(OnEnd);
    }

    private void OnEnd()
    {
        ApplyAttempt();
        TryTrigger();
    }
}
