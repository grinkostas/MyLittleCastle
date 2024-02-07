using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using StaserSDK.Stack;
using Zenject;

public class CollectingState : HelperState
{
    [SerializeField] private float _minItemsToNextState;
    [SerializeField] private List<ItemType> _itemTypesToCollect;
    [SerializeField] private float _errorPathReloadDelay;
    [SerializeField] private float _reloadTime;

    private ResourcePlace _currentPlace;

    protected override void OnEnter()
    {
        NextPlace();
        Helper.Stack.CountChanged += OnHelperStackCountChanged;
    }

    private void OnHelperStackCountChanged(int count)
    {
        ActualizeTimer();
    }

    private void ActualizeTimer()
    {
        DOTween.Kill(this);
        if(Helper.Stack.ItemsCount >= _minItemsToNextState)
            return;
        
        DOVirtual.DelayedCall(_reloadTime, NextPlace).SetId(this);
    }
    
    private void NextPlace()
    {
        if (_currentPlace != null)
            _currentPlace.Finished -= OnPlaceFinished;
        
        _currentPlace = GetRandomPlace();
        if (_currentPlace == null)
        {
            DOTween.Kill(_errorPathReloadDelay);
            DOVirtual.DelayedCall(_errorPathReloadDelay, NextPlace).SetId(_errorPathReloadDelay);
            return;
        }
        
        ActualizeTimer();
        _currentPlace.Finished += OnPlaceFinished;
        Helper.Handler.SetDestination(_currentPlace.TaskPoint.position);
    }
    private ResourcePlace GetRandomPlace()
    {
        return Helper.HelperHouse.ResourcePlaces.FindAll(x => x.AvailableToHelp && x.Active && _itemTypesToCollect.Contains(x.Type)).Random();
    }

    private void OnPlaceFinished()
    {
        if (Helper.Stack.ItemsCount >= _minItemsToNextState)
        {
            Exit();
            return;
        }
        
        NextPlace();
        
    }
    protected override void OnExit()
    {
        if (_currentPlace != null)
            _currentPlace.Finished -= OnPlaceFinished;
        
        Helper.Stack.CountChanged -= OnHelperStackCountChanged;
        DOTween.Kill(this);
        DOTween.Kill(_errorPathReloadDelay);
        _currentPlace = null;
    }
}
