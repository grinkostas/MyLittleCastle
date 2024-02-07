using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NaughtyAttributes;
using StaserSDK.Stack;
using StaserSDK.Utilities;
using Zenject;

public class StackSaver : Saver<Dictionary<ItemType, int>>
{
    [SerializeField] private StackProvider _stackProvider;
    [SerializeField, ShowIf(nameof(_moveItem))] private Transform _parent;
    [SerializeField] private bool _moveItem = true;
    [SerializeField, ShowIf(nameof(_moveItem))] private bool _hasStackLocator;
    [SerializeField, ShowIf(nameof(_hasStackLocator))] private StackItemLocator _stackItemLocator;
    [SerializeField] private bool _claim;
    [SerializeField] private bool _active;
    [SerializeField] private bool _returnToPool;

    [Inject] private ResourceController _resourceController;
    protected override Dictionary<ItemType, int> DefaultValue => new Dictionary<ItemType, int>();
    
    private void OnEnable()
    {
        ApplySave();
    }

    private void ApplySave()
    {
        var saves = GetSave();
        foreach (var save in saves)
        {
            if(save.Key == ItemType.Any || save.Key == ItemType.None)
                continue;

            for (int i = 0; i < save.Value; i++)
            {
                var stackItem = _resourceController.GetInstance(save.Key);
                var stackItemTransform = stackItem.transform;
                stackItemTransform.SetParent(_parent);
                stackItemTransform.localRotation = Quaternion.identity;
                stackItemTransform.localPosition = Vector3.zero;
                _stackProvider.Interface.Add(stackItem);
                
                if(_claim)
                    stackItem.Claim();
                
                if(_returnToPool)
                    stackItem.Pool.Return(stackItem);
                
                if(_moveItem == false)
                    continue;
                if (_hasStackLocator)
                    stackItemTransform.localPosition =_stackItemLocator.GetDeltaByIndex(i, true);
                
            }
        }
    }

    protected override Dictionary<ItemType, int> GetSaveData()
    {
        var dictionary = new Dictionary<ItemType, int>();
        foreach (var item in _stackProvider.Interface.Items)
        {
            dictionary.Add(item.Key, item.Value.Value);
        }

        return dictionary;
    }

    protected override bool NeedToSave() => true;




}
