using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK;

public class PlayerAnimationSpeedView : CheatCountView
{
    [SerializeField] private MovementAnimator _movementAnimator;

    protected override void ActualizeValue(int value)
    {
        _countText.text = (value/4.0f).ToString();
    }

    protected override void PlusButtonClick()
    {
        _movementAnimator._maxSpeed += _step / 4.0f;
    }
    
    protected override void MinusButtonClick()
    {
        _movementAnimator._maxSpeed -= _step / 4.0f;
    }
}
