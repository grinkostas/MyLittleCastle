using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class EnemySpot : BubbleListener
{
    [SerializeField] private string _id;
    [Inject] public SpotManager SpotManager { get; }

    private Enemy _currentEnemy = null;
    public bool Busied { get; private set; }
    public string Id => _id;
    

    private void OnEnable()
    {
        if(Inside)
            return;
        SpotManager.Add(this);
        BubbleTrigger.OnEnterBubble.Once(_=> SpotManager.Remove(this));
    }

    private void OnDisable()
    {
        SpotManager.Remove(this);
    }

    public void Use(Enemy enemy)
    {
        if(Busied)
            return;
        Busied = true;
        _currentEnemy = enemy;
    }

    public void Leave(Enemy enemy)
    {
        if(_currentEnemy != enemy)
            return;
        Busied = false;
        _currentEnemy = null;
    }

}
    
