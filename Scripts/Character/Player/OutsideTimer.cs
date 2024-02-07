using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NepixSignals;
using StaserSDK.Upgrades;
using StaserSDK.Utilities;
using UnityEngine.Events;
using Zenject;

public class OutsideTimer : BubbleListener, IProgressible
{
    [SerializeField] private UpgradeValue _torchUpgrade;
    [SerializeField] private float _regenerationSpeed = 2;
    [SerializeField] private WeaponController _weaponController;
    [SerializeField] private float _startRegenerationDelayTime;

    [Inject] private Timer _timer;

    private TimerDelay _startRegenerationDelay;
    
    private float _wastedTime = 0;

    private bool _disabled = false;
    
    public bool InsideBubble { get; private set; } = true;
    
    private float WastedTime
    {
        get => _wastedTime;
        set
        {
            _wastedTime = value;
            ProgressChanged?.Invoke(Progress);
        }
    }

    private float OutsideSafeTime => Modifier.GetValue(_torchUpgrade.Value);

    public ModifierValue<float, FloatModifier> Modifier { get; } = new();

    public float Progress => _wastedTime / OutsideSafeTime;

    public TheSignal OnEnterBubble { get; } = new();
    public TheSignal OnExitBubble { get; } = new();
    public TheSignal Ended { get; } = new();
    public UnityAction<float> ProgressChanged { get; set; }
    
    protected override void OnEnableInternal()
    {
        Modifier.Changed.On(OnModifierChange);
    }
    
    protected override void OnDisableInternal()
    {
        Modifier.Changed.Off(OnModifierChange);
    }

    private void OnModifierChange()
    {
        ProgressChanged?.Invoke(Progress);
    }
    protected override void OnInsideBubble()
    {
        InsideBubble = true;
        OnEnterBubble.Dispatch();
        DOTween.Kill(this);
        DOVirtual.Float(WastedTime, 0, _regenerationSpeed * Progress, value =>
        {
            WastedTime = value;
            _disabled = false;
            _weaponController.Enable();
        })
            .SetEase(Ease.Linear)
            .SetId(this);
    }


    protected override void OnOutsideBubble()
    {
        InsideBubble = false;
        OnExitBubble.Dispatch();
        DOTween.Kill(this);
        DOVirtual.Float(WastedTime, OutsideSafeTime, OutsideSafeTime-WastedTime, value =>
        {
            if (value > OutsideSafeTime - 0.001f)
                value = OutsideSafeTime;
            WastedTime = value;
        }).SetId(this)
            .OnComplete(() =>
            {
                Ended.Dispatch();
                _disabled = true;
                _weaponController.Disable();
            });
    }
}
