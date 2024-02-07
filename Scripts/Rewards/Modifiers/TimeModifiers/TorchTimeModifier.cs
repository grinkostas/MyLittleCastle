using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Zenject;

public class TorchTimeModifier : TimeModifier
{
    [Inject, UsedImplicitly] public Player Player { get; }
    private FloatModifier _modifier;
    public override float DefaultDuration => Settings.InfiniteTorceDuration;
    
    public TorchTimeModifier(string id, FloatModifier modifier) : base(id)
    {
        _modifier = modifier;
    }

    protected override void OnApplyModifier()
    {
        Player.OutsideTimer.Modifier.AddModifier(this, _modifier);
    }

    protected override void OnRemoveModifier()
    {
        Player.OutsideTimer.Modifier.RemoveModifier(this);
    }
}
