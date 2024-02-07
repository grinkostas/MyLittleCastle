using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class PlayerSpeedCheatView : CheatCountView
{
    [Inject] private Player _player;
    protected override void ActualizeValue(int value)
    {
        float speedChange = value / 4.0f;
        _countText.text = (value/4.0f).ToString();
        _player.Speed.RemoveModifier(this);
        _player.Speed.AddModifier(this, new FloatModifier(speedChange, NumericAction.Add));
    }
}
