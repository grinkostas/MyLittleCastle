
using JetBrains.Annotations;
using StaserSDK.Interactable;
using StaserSDK.Upgrades;
using UnityEngine;
using Zenject;

public class WeaponUpgradeCondition : InteractCondition
{
    [SerializeField] private Destructible _destructible;
    [SerializeField] private View _view;
    [SerializeField] private int _targetLevel;
    
    [Inject, UsedImplicitly] public Player Player { get; }
    
    public override bool CanInteract(InteractableCharacter character)
    {
        var weapon = Player.Equipment.WeaponController.GetWeaponByType(_destructible.TargetWeaponType);
        if (weapon.DamageUpgrade.CurrentLevel >= _targetLevel)
        {
            _view.Hide();
            return true;
        }

        _view.Show();
        return false;
    }
}
