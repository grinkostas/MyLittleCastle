using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using StaserSDK.Interactable;
using StaserSDK.Stack;
using UnityEngine.Events;
using Zenject;

public class ResourcePlace : Destructible
{
    [SerializeField] private EquippedCharacterZone _actionZone;
    [SerializeField] private GameObject _checkActiveObject;
    [SerializeField] private Transform _model;
    [SerializeField] private ItemType _placeType;
    [SerializeField] private int _capacity;
    [SerializeField] private bool _taskTarget = true;
    [SerializeField] private bool _finishOnDisable = true;
    
   [Inject, UsedImplicitly] public ResourceController ResourceController { get; }
    
    public bool AvailableToHelp => _checkActiveObject.activeInHierarchy && _taskTarget;
    public bool Active { get; private set; } = true;
    public bool DebugActive = true;

    public Transform TaskPoint => _actionZone.transform;
    public UnityAction Finished { get; set; }
    public int Capacity => _capacity;

    public ItemType Type => _placeType;
    public Transform Model => _model;

    [Inject]
    public void Inject()
    {
        ResourceController.AddPlace(this);
    }

    private void OnEnable()
    {
        ResourceController.AddPlace(this);
        _actionZone.OnExit += OnExit;
        _actionZone.OnInteractInternal += OnInteract;
    }

    private void OnDisable()
    {
        ResourceController.RemovePlace(this);
        _actionZone.OnExit -= OnExit;
        _actionZone.OnInteractInternal -= OnInteract;
        if(_finishOnDisable)
            Finished?.Invoke();
    }

    private void OnInteract(EquippedCharacter equippedCharacter)
    {
        Use(equippedCharacter);
    }

    private void OnExit(InteractableCharacter _)
    {
        Using = false;
    }

    protected override bool UseCondition(Weapon weapon)
    {
        return _actionZone.IsCharacterInside;
    }

    protected override void OnHealthChanged(int health)
    {
        if (health <= 0)
        {
            Active = false;
            DebugActive = false;
            Finished?.Invoke();
        }
        else
        {
            Active = true;
            DebugActive = true;
        }
    }
    
    protected override void OnRespawn()
    {
        Active = true;
        DebugActive = true;
    }

    
    public void UseTask()
    {
    }


}
