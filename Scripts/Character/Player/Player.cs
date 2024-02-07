using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK;
using StaserSDK.Interactable;

public class Player : MonoBehaviour
{
    [SerializeField] private EquippedCharacter _equippedCharacter;
    [SerializeField] private StackableCharacter _stackableCharacter;
    [SerializeField] private AnimatorLinker _animatorLinker;
    [SerializeField] private Transform _model;
    [SerializeField] private CharacterControllerMovement _movement;
    [SerializeField] private CharacterControllerMovement _characterControllerMovement;
    [SerializeField] private BubbleTrigger _bubbleTrigger;
    [SerializeField] private OutsideTimer _outsideTimer;
    [SerializeField] private Health _health;
    
    public Animator Animator => _animatorLinker.Animator;
    public Transform Model => _model;
    public MovementHandler Movement => _movement.Handler;
    public CharacterControllerMovement CharacterControllerMovement => _movement;
    public EquippedCharacter Equipment => _equippedCharacter;
    public StackableCharacter Stack => _stackableCharacter;
    public ModifiedValue<float> Speed => _characterControllerMovement.Speed;
    public BubbleTrigger Trigger => _bubbleTrigger;
    public OutsideTimer OutsideTimer => _outsideTimer;
    public Health Health => _health;

}
