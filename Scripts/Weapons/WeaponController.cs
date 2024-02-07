using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Events;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private List<Weapon> _weapons;
    [SerializeField] private Health _health;

    private bool _enabled = true;
    private Weapon _currentWeaponInUse;

    public bool DestructibleInitialized { get; private set; } = false;
    public Destructible CurrentDestructible { get; private set; }

    public List<Weapon> Weapons => _weapons;

    public bool Enabled
    {
        get => _enabled;
        private set
        {
            if (_enabled == value)
                return;
            _enabled = value;
            ChangedActive?.Invoke(_enabled);
        }
    }

    public UnityAction<bool> ChangedActive { get; set; }
    public UnityAction UsedDisabledWeapon { get; set; }

    public bool TryGetWeapon(WeaponType weaponType, out Weapon weapon)
    {
        weapon = null;
        if (Enabled == false)
        {
            UsedDisabledWeapon?.Invoke();
            return false;
        }

        if (_weapons.Count(x => x.InUse && x.Type != weaponType) > 0)
            return false;

        weapon = _weapons.Find(x => x.Type == weaponType);
        if (weapon == null)
            return false;

        return true;
    }

    public Weapon GetWeaponByType(WeaponType type)
    {
        return _weapons.Find(x => x.Type == type);
    }

    public bool TryGetAndSelectWeapon(Destructible destructible, WeaponType weaponType, out Weapon weapon)
    {
        weapon = null;
        if (Enabled == false)
        {
            UsedDisabledWeapon?.Invoke();
            return false;
        }
        
        weapon = _weapons.Find(x => x.Type == weaponType);
        
        if (weapon == null)
            return false;
        
        if (weapon == _currentWeaponInUse)
            return false;

        DestructibleInitialized = true;
        CurrentDestructible = destructible;
        
        _currentWeaponInUse = weapon;
        _currentWeaponInUse.EndedUsing += OnEndUsing;
        return true;
    }


    private void OnEndUsing(Weapon weapon)
    {
        if(weapon != _currentWeaponInUse)
            return;
        
        _currentWeaponInUse.EndedUsing -= OnEndUsing;
        _currentWeaponInUse = null;
        
        DestructibleInitialized = false;
        CurrentDestructible = null;
    }
    public void UseWeapon()
    {
        if (_currentWeaponInUse == null || Enabled == false)
            return;
        if (_health != null && _health.Health == 0)
        {
            _currentWeaponInUse.AbortUse();
            return;
        }

        _currentWeaponInUse.UseWeapon();
    }

    public void Disable()
    {
        foreach (var weapon in _weapons)
        {
            if(weapon.gameObject.activeSelf == false)
                return;
            weapon.AbortUse();
            weapon.gameObject.SetActive(false);
        }
        Enabled = false;
    }

    public void Enable()
    {
        foreach (var weapon in _weapons)
        {
            if(weapon.gameObject.activeSelf)
                return;
            weapon.gameObject.SetActive(true);
        }
        Enabled = true;
    }
}
