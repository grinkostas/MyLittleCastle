using System;
using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using StaserSDK;
using StaserSDK.Upgrades;
using UnityEngine.Events;
using Zenject;
using StaserSDK.Utilities;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponType _weaponType;
    [SerializeField] private Movement _movemen;
    [SerializeField] private UpgradeValue _damage;
    [SerializeField] private float _endUseDelay;
    [Header("Animations")]
    [SerializeField] private AnimatorLinker _animatorLinker;
    [SerializeField] private string _useAnimation = "Attack";
    
    private Tween _reloadTimer = null;

    private Animator Animator => _animatorLinker.Animator;
    
    public int Damage => DamageModifier.GetValue(_damage.ValueInt);
    public UpgradeValue DamageUpgrade => _damage;
    public ModifierValue<int, IntModifier> DamageModifier { get; } = new();
    public WeaponType Type => _weaponType;
    
    public bool InUse { get; private set; } = false;

    public UnityAction<Weapon> StartUsing { get; set; }
    public UnityAction<Weapon> Used { get; set; }
    public UnityAction<Weapon> EndedUsing { get; set; }

    private void OnEnable()
    {
        if (_movemen != null)
            _movemen.Handler.OnStartMove += AbortUse;
        Refresh();
       
    }

    private void OnDisable()
    {
        if (_movemen != null)
            _movemen.Handler.OnStartMove -= AbortUse;
        Refresh();
    }

    private void Refresh()
    {
        InUse = false;
        DOTween.Kill(this);
    }

    public void Use()
    {
        if(InUse || enabled == false || gameObject.activeInHierarchy == false)
            return;
        Animator.SetBool(_useAnimation, true);
        InUse = true;
        StartUsing?.Invoke(this);
        
    }

    public void AbortUse()
    {
        DOTween.Kill(this);
        EndUse();
    }

    public void UseWeapon()
    {
        DOTween.Kill(this);
        Used?.Invoke(this);
        DOVirtual.DelayedCall(_endUseDelay, EndUse).SetId(this);
    }
    
    private void EndUse()
    {
        Refresh();
        Animator.SetBool(_useAnimation, false);
        EndedUsing?.Invoke(this);
    }
}
