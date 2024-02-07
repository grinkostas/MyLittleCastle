using System.Collections.Generic;
using StaserSDK.Stack;
using UnityEngine;

public class BuyZoneConstSaver : BuyZoneSaver
{
    [SerializeField] private List<CostData> _defaultCostData;
    protected override Dictionary<ItemType, int> DefaultValue { get; }
    
    protected override Dictionary<ItemType, int> GetSaveData()
    {
        Dictionary<ItemType, int> dictionary = new();
        foreach (var costData in _defaultCostData)
        {
            dictionary.Add(costData.Resource, costData.Amount);
        }

        return dictionary;
    }

    protected override bool NeedToSave()
    {
        return false;
    }
}
