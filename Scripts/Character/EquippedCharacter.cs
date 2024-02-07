using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Interactable;

public class EquippedCharacter : InteractableCharacter
{
    [SerializeField] private WeaponController _weaponController;
    public WeaponController WeaponController => _weaponController;
    
    
    private void OnDisable()
    {
        foreach (var weapon in _weaponController.Weapons)     
        {
            weapon.AbortUse();
        }
    }
}
