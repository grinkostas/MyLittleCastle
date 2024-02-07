using StaserSDK.Upgrades;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


public class StackCheatUpgrade : MonoBehaviour
{
    [SerializeField] private Button _upgradeButton;
    
    [Inject] private UpgradesController _upgradesController;
    [Inject] public Player Player { get; }
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
        var upgradeValue = Player.Stack.MainStack.CapacityUpgradeValue;
        if(upgradeValue.ConstValue)
            return;
        var model = _upgradesController.GetModel(upgradeValue.Upgrade);
        if (model.CanLevelUp())
            model.LevelUp();
    }
}
