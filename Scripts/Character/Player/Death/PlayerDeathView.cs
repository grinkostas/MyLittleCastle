using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using StaserSDK.Stack;
using Zenject;

public class PlayerDeathView : MonoBehaviour
{
    [SerializeField] private View _view;
    [SerializeField] private float _showDelay;
    [SerializeField] private PlayerDeath _playerDeath;
    [SerializeField] private PlayerRespawn _playerRespawn;
    [SerializeField] private MonoPool<ResourceView> _resourceViewPrefabsPool;

    [Inject] private DiContainer _container;
    private List<ResourceView> _takenViews = new List<ResourceView>();
    
    private void Awake()
    {
        _resourceViewPrefabsPool.Initialize(_container);
    }

    private void OnEnable()
    {
        _playerDeath.TookItems.On(OnPlayerDie);
    }
    
    private void OnDisable()
    {
        _playerDeath.TookItems.Off(OnPlayerDie);
    }
    
    private void OnPlayerDie(List<StackItem> items)
    {
        ClearPool();
        Debug.Log(items.Count);
        var grouped = items.GroupBy(item => item.Type)
            .Select(group => new KeyValuePair<ItemType,int>(group.Key, group.Count()));
        
        foreach (var pair in grouped)
        {
            var view =_resourceViewPrefabsPool.Get().Init(pair.Key, pair.Value);
            _takenViews.Add(view);
        }

        DOVirtual.DelayedCall(_showDelay, _view.Show);
    }

    public void Hide()
    {
        _view.HideComplete.Once(ClearPool);
        _view.Hide();
    }

    private void ClearPool()
    {
        foreach (var takenView in _takenViews)
        {
            takenView.Pool.Return(takenView);
        }
        _takenViews.Clear();
    }

    public void Respawn()
    {
        _playerRespawn.Respawn();
    }
}
