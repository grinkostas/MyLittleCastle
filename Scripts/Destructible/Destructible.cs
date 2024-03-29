using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using NepixSignals;
using UnityEngine;
using UnityEngine.Events;

public abstract class Destructible : MonoBehaviour, IProgressible
{
    [SerializeField] private int _health;
    [SerializeField] private int _priority;
    [SerializeField] private WeaponType _weaponTypeToDamage;
    
    [SerializeField, ShowIf(nameof(_debug))] private int _damage = 0;
    [SerializeField] private bool _debug;
    protected bool Using { get; set; } = false;
    
    public int MaxHealth => _health;
    public int Health => MaxHealth - _damage;

    public int Damage
    {
        get => _damage;
        set
        {
            var startDamage = _damage;
            _damage = value;
            if(startDamage < value)
                Damaged.Invoke(value - startDamage);
            ProgressChanged?.Invoke((float)Health/MaxHealth);
            HealthChanged?.Invoke();
            if(_damage == 0)
                Respawned.Dispatch();
            if(_damage == MaxHealth)
                Died?.Invoke();
        }
    }

    public bool IsAlive => Health > 0;
    public int Priority => _priority;
    public WeaponType TargetWeaponType => _weaponTypeToDamage;

    public UnityAction<int> Damaged { get; set; }
    public UnityAction HealthChanged { get; set; }
    
    public UnityAction Died { get; set; }
    public TheSignal Respawned { get; } = new();

    public Weapon CurrentWeaponInUse { get; private set; }
    
    public UnityAction<float> ProgressChanged { get; set; }

    public void Respawn()
    {
        Damage = 0;
        Using = false;
        OnRespawn();
    }

    protected virtual void OnRespawn()
    {
    }

    public void Use(EquippedCharacter equippedCharacter)
    {
        if(IsAlive == false || Using)
            return;

        var weaponController = equippedCharacter.WeaponController;
        if (weaponController.DestructibleInitialized && weaponController.CurrentDestructible.Priority >= Priority)
        {
            if (weaponController.CurrentDestructible.TargetWeaponType == TargetWeaponType)
                UseWeapon(weaponController.GetWeaponByType(TargetWeaponType));
            return;
        }
        
        if (weaponController.TryGetAndSelectWeapon(this, _weaponTypeToDamage, out Weapon weapon))
        {
            UseWeapon(weapon);
        }
    }

    public void UseWeapon(Weapon weapon)
    {
        if(IsAlive == false || Using)
            return;
        
        CurrentWeaponInUse = weapon;
        Using = true;
        weapon.Used += OnWeaponUse;
        weapon.EndedUsing += OnWeaponEndUse;
        weapon.Use();
    }

    public void AbortWeaponUse(Weapon weapon)
    {
        if (weapon == CurrentWeaponInUse)
        {
            OnWeaponEndUse(weapon);
            weapon.AbortUse();
        }
    }

    private void OnWeaponUse(Weapon weapon)
    {
        if (IsAlive == false || UseCondition(weapon) == false)
        {
            OnWeaponEndUse(weapon);
            return;
        }
        CurrentWeaponInUse = weapon;
        int damage = weapon.Damage;
        if (Health - damage < 0)
            damage = Health;

        ApplyDamage(damage);
        
        if (IsAlive == false)
        {
            CurrentWeaponInUse.AbortUse();
            CurrentWeaponInUse = null;
            return;
        }
        
        weapon.Use();
    }

    public void ApplyDamage(int damage)
    {
        if(IsAlive == false)
            return;
        Damage += damage;
        OnHealthChanged(Health);
    }

    public void Heal(int amount)
    {
        if(_damage == 0)
            return;
        Damage = Mathf.Max(_damage - amount, 0);
        OnHealthChanged(Health);
    }

    public void SetHealth(int amount)
    {
        amount = Mathf.Min(MaxHealth, amount);
        _damage = MaxHealth - amount;
        ProgressChanged?.Invoke((float)Health/MaxHealth);
        HealthChanged?.Invoke();
        if(_damage == 0)
            Respawned.Dispatch();
        OnHealthChanged(Health);
    }
    

    private void OnWeaponEndUse(Weapon weapon)
    {
        Using = false;
        CurrentWeaponInUse = null;
        weapon.Used -= OnWeaponUse;
        weapon.EndedUsing -= OnWeaponEndUse;
    }

    protected abstract bool UseCondition(Weapon weapon);

    protected virtual void OnHealthChanged(int health)
    {
    }

    private void DebugLog(string log)
    {
        if(_debug == false)
            return;
        Debug.Log(log);
    }

}
