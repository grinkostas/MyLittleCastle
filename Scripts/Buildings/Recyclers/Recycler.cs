using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using StaserSDK.Interactable;
using StaserSDK.Stack;
using StaserSDK.Utilities;
using UnityEngine.Events;
using Zenject;

public class Recycler : StackTaker
{
    [SerializeField] private StackProvider _sourceStack;
    [SerializeField] private StackProvider _productionStack;
    [SerializeField] private StackItemLocator _stackItemLocator;
    [SerializeField] private int _itemsToRecycle;
    [SerializeField] private ItemType _takeType;
    [SerializeField] private float _addDelay;
    [SerializeField] private float _startProductDelay;
    [SerializeField] private float _recycleItemTime;
    [SerializeField] private ItemType _productType;
    [SerializeField] private List<RecycleCondition> _recycleConditions;
    [SerializeField] private float _defaultActualizeDelay = 3.0f;

    [Inject] private ResourceController _resourceController;
    [Inject] private DiContainer _diContainer;
    [Inject] private Timer _timer;
    
    private bool _productionInProcess = false;

    private int _delayedItemsCount = 0;

    private float RecycleTime => RecycleTimeModifier.GetValue(_recycleItemTime);
    public ModifierValue<float, FloatModifier> RecycleTimeModifier { get; } = new();

    public StackProvider ProductionStack => _productionStack;
    
    public ItemType ProductType => _productType;
    public ItemType SourceType => _takeType;
    private int DelayedItemsCount
    {
        get => _delayedItemsCount;
        set
        {
            _delayedItemsCount = Mathf.Clamp(value, 0, int.MaxValue);
        }
    }
    protected override Vector3 DestinationDelta => _stackItemLocator.GetNextDelta(DelayedItemsCount);
    

    public UnityAction OnStartRecycle;
    public UnityAction OnEndRecycle;

    public override ItemType GetTypeToTake(StackableCharacter interactableCharacter) => _takeType;
    
    private void Start()
    {
        StartProduct();
        _timer.ExecuteWithDelay(DelayedActualize, _defaultActualizeDelay);
    }

    private void DelayedActualize()
    {
        StartProduct();
        _timer.ExecuteWithDelay(DelayedActualize, _defaultActualizeDelay);
    }

    public override bool TakePredicate()
    {
        return (_sourceStack.Interface.ItemsCount + DelayedItemsCount) < _sourceStack.Interface.MaxSize;
    }

    protected override void OnTakeItem(StackItem stackItem)
    {
        DelayedItemsCount++;
        _timer.ExecuteWithDelay(() =>
        {
            if (_sourceStack.Interface.TryAdd(stackItem))
            {
                StartProduct();
            }

            DelayedItemsCount--;
        }, _addDelay);
        
    }
    private bool PassConditions()
    {
        foreach (var condition in _recycleConditions)
        {
            if (condition.CanRecycle() == false)
                return false;
        }

        return true;
    }

    private Tween _startProductTween;
    
    private void StartProduct()
    {
        if(CanStartProduct() == false)
            return;
        
        if(_startProductTween is { active: true })
            return;
        
        if(_productionInProcess)
            return;
        
        OnStartRecycle?.Invoke();
        _startProductTween = DOVirtual.DelayedCall(_startProductDelay, Product).SetId(this);
    }

    private bool CanStartProduct()
    {
        if (_sourceStack.Interface.ItemsCount < _itemsToRecycle
            || _sourceStack.Interface.Items[_takeType].Value < _itemsToRecycle
            || PassConditions() == false
            || _productionStack.Interface.ItemsCount >= _productionStack.Interface.MaxSize)
            return false;
        return true;
    }
    
    private void Product()
    {
        if (CanStartProduct() == false)
        {
            StopProduction();
            return;
        }

        StartProductItem();
        TakeItemsToProduct();
        ProductItem();
    }

    private void StartProductItem()
    {
        DOTween.Kill(this);
        _recycleConditions.ForEach(x=>x.HandleConditionPassed());
        _productionInProcess = true;
    }

    private void TakeItemsToProduct()
    {
        for (int i = 1; i <= _itemsToRecycle; i++)
        {
            if (_sourceStack.Interface.TryTake(_takeType, out StackItem takeItem, transform) == false)
            {
                break;
            }
        }
    }
    
    private void ProductItem()
    {
        DOVirtual.DelayedCall(RecycleTime, () =>
        {
            var resource = _resourceController.GetInstance(_productType);
            resource.transform.SetParent(_productionStack.GameObject.transform, false);
            resource.Claim();
            _productionStack.Interface.TryAdd(resource);

            if (CanStartProduct() == false)
            {
                _productionInProcess = false;
                DOVirtual.DelayedCall(RecycleTime, Product).SetId(this);
                return;
            }

            Product();
        }).SetId(this);
    }
    
    private void StopProduction()
    {
        _productionInProcess = false;
        DOTween.Kill(this);
        OnEndRecycle?.Invoke();
    }

}
