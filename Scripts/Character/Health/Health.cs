using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using StaserSDK.Interactable;
using UnityEngine.Events;

public class Health : Destructible
{
    [SerializeField, HideIf(nameof(_externalUse))] private EquippedCharacterZone _characterZone;
    [SerializeField] private bool _externalUse;
    
    
    private void OnEnable()
    {
        if(_externalUse)
            return;
        _characterZone.OnInteractInternal += OnInteract;
        _characterZone.OnExit += OnCharacterExit;
    }

    private void OnDisable()
    {
        if(_externalUse)
            return;
        _characterZone.OnInteractInternal -= OnInteract;
        _characterZone.OnExit -= OnCharacterExit;
    }

    private void OnInteract(EquippedCharacter equippedCharacter)
    {
        if(_externalUse)
            return;
        Use(equippedCharacter);
    }

    private void OnCharacterExit(InteractableCharacter character)
    {
        if(CurrentWeaponInUse == null)
            return;
        AbortWeaponUse(CurrentWeaponInUse);
    }
    
    protected override bool UseCondition(Weapon weapon)
    {
        return _characterZone.IsCharacterInside && _characterZone.Character.enabled && enabled && weapon.enabled;
    }

}
