using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using StaserSDK.Extentions;
using StaserSDK.Stack;
using StaserSDK.Upgrades;
using UnityEngine.Events;
using Zenject;

public abstract class StackBase : MonoBehaviour, IStack
{
    [SerializeField, HideIf(nameof(_capacityFromUpgrade))] private int _maxSize;
    [SerializeField] private bool _capacityFromUpgrade;
    [SerializeField, ShowIf(nameof(_capacityFromUpgrade))] private UpgradeValue _capacityUpgrade;
    [SerializeField] private bool _storeAnyType;
    [SerializeField, HideIf(nameof(_storeAnyType))] private List<ItemType> _storedTypes;

    [Inject] private UpgradesController _upgradesController;
    private Dictionary<ItemType, IntReference> _items = new();
    private bool _initialized = false;
    private int _count = 0;

    private List<object> _blockers = new List<object>();

    public int ItemsCount => _count;
    public ModifierValue<int, IntModifier> SizeModifier { get; } = new();
    public int MaxSize => _capacityFromUpgrade ? SizeModifier.GetValue(_capacityUpgrade.ValueInt) :  _maxSize;
    public List<StackItem> SourceItems { get; } = new List<StackItem>();

    public UpgradeValue CapacityUpgradeValue => _capacityUpgrade;

    public Dictionary<ItemType, IntReference> Items
    {
        get
        {
            if(_initialized == false)
                Initialize();
            return _items;
        }
    }

    public bool Disabled => _blockers.Count > 0;
    
    public UnityAction<StackItemData> AddedItem { get; set; }
    public UnityAction<StackItemData> TookItem { get; set; }
    public UnityAction<ItemType, int> TypeCountChanged { get; set; }
    public UnityAction<int> CountChanged { get; set; }

    private void Awake()
    {
        Initialize();
    }

    public void Clear()
    {
        var items = new List<StackItem>(SourceItems);
        SourceItems.Clear();
        Items.Clear();
        _initialized = false;
        Initialize();
        _count = 0;
        CountChanged?.Invoke(0);

        foreach (var item in items)
            TookItem?.Invoke(new StackItemData(item));
        
        foreach (var item in items.GroupBy(x => x.Type))
            TypeCountChanged?.Invoke(item.Key, -item.Count());
        
       
        
    }
    
    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        if(_initialized)
            return;
        InitializeItemDictionary(_items, () => new IntReference());
        _initialized = true;
    }

    protected void InitializeItemDictionary<T>(Dictionary<ItemType, T> dictionary, Func<T> getDefaultValue)
    {
        if (_storeAnyType || _storedTypes.Has(x=>x == ItemType.Any))
        {
            foreach (ItemType itemType in Enum.GetValues(typeof(ItemType)))
            {
                dictionary.Add(itemType, getDefaultValue());
            }
        }
        else
        {
            foreach (ItemType suit in _storedTypes)
            {
                dictionary.Add(suit, getDefaultValue());
            }
        }
    }

    public bool StoreType(ItemType itemType)
    {
        foreach (var storedType in _storedTypes)
        {
            if (storedType == itemType || storedType == ItemType.Any)
                return true;
        }

        return false;
    }

    public bool TryAdd(StackItem stackItem)
    {
        if (gameObject.activeInHierarchy == false)
            return false;
        Initialize();
        if (Disabled)
            return false;
        if (_count + stackItem.Amount > MaxSize)
            return false;

        Add(stackItem);
        return true;
    }

    public void Add(StackItem stackItem)
    {
        Initialize();
        var countRef = Items[stackItem.Type];
        countRef.Value += stackItem.Amount;
        
        SourceItems.Add(stackItem);
        _count = CalculateCount();
        
        OnAddItem(stackItem);
        AddedItem?.Invoke(new StackItemData(stackItem));
        TypeCountChanged?.Invoke(stackItem.Type, stackItem.Amount);
        CountChanged?.Invoke(_count);
    }

    private int CalculateCount()
    {
        return Items.Sum(x => x.Value.Value);
    }
    
    protected virtual void OnAddItem(StackItem stackItem)
    {
    }

    public bool TryAddRange(ItemType type, int count)
    {
        if (Disabled)
            return false;
        if (gameObject.activeInHierarchy == false)
            return false;
        Initialize();
        if (_count + count > MaxSize)
            return false;

        var countRef = Items[type];
        countRef.Value += count;
        _count = CalculateCount();
        TypeCountChanged?.Invoke(type, count);
        CountChanged?.Invoke(_count);
        return true;
    }

    public bool TryTake(ItemType itemType, out StackItem stackItem, Transform destination, StackItemDataModifier modifier = new StackItemDataModifier())
    {
        stackItem = null;
        if (itemType == ItemType.Any)
        {
            if (TryGetLastType(out itemType) == false)
                return false;
        }
        
        if (gameObject.activeInHierarchy == false)
            return false;
        
        var itemCountRef = Items[itemType];
        if (itemCountRef.Value <= 0)
            return false;
        itemCountRef.Value--;
        if (TryGetInstance(itemType, out stackItem) == false)
            return false;

        SourceItems.Remove(stackItem);
        _count = CalculateCount();
        
        OnTakeItem(stackItem);
        TookItem?.Invoke(new StackItemData(stackItem).AddDestination(destination).ApplyModifier(modifier));
        TypeCountChanged?.Invoke(itemType, -stackItem.Amount);
        CountChanged?.Invoke(_count);
        return true;
    }
    
    protected virtual void OnTakeItem(StackItem stackItem)
    {
    }

    protected abstract bool TryGetInstance(ItemType type, out StackItem stackItem);
    protected abstract IEnumerable<StackItem> GetInstances(ItemType type, int count);

    public bool TryGetLastType(out ItemType itemType)
    {
        itemType = ItemType.None;
        if (SourceItems.Count == 0)
        {
            foreach (var itemsPair in Items)
            {
                if (itemsPair.Value.Value > 0)
                {
                    itemType = itemsPair.Key;
                    break;
                }
            }

            if (itemType == ItemType.None)
                return false;
            return true;
        }
        
        itemType = SourceItems.Last().Type;
        return true;
    }
    
    public bool TryTakeLast(out StackItem stackItem, Transform destination, StackItemDataModifier modifier = new StackItemDataModifier())
    {
        stackItem = null;
        if (gameObject.activeInHierarchy == false)
            return false;
        if (TryGetLastType(out ItemType takeType) == false)
            return false;
        return TryTake(takeType, out stackItem, destination, modifier);
    }

    public bool TrySpend(ItemType type, int amount)
    {
        if (gameObject.activeInHierarchy == false)
            return false;
        
        var countRef = Items[type];
        
        if (countRef.Value < amount)
            return false;
        
        countRef.Value -= amount;

        _count = Items.Sum(x=>x.Value.Value);
        TypeCountChanged?.Invoke(type, -amount);
        CountChanged?.Invoke(_count);
        return true;
    }

    public bool TrySpend(ItemType type, int amount, out IEnumerable<StackItem> spendItems)
    {
        spendItems = default;
        if (gameObject.activeInHierarchy == false)
            return false;
        if (TrySpend(type, amount) == false)
            return false;
        spendItems = GetInstances(type, amount); 
        return true;
    }

    public void DisableCollect(object sender)
    {
        if (_blockers.Contains(sender))
            return;
        _blockers.Add(sender);
    }

    public void EnableCollect(object sender)
    {
        _blockers.Remove(sender);
    }
}
