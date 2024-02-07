using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Zenject;

public class CheatBuyZone : MonoBehaviour
{
    [SerializeField] private Button _upgradeButton;
    [Inject] private BuyZoneController _buyZoneController;
    
    private void OnEnable()
    {
        _upgradeButton.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        _upgradeButton.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    { 
        _buyZoneController.GetCurrentAvailableZone().ForceBuy();
    }
}
