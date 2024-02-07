using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Zenject;

public class Enemy : MonoBehaviour
{
    [SerializeField] private AnimatorLinker _animatorLinker;
    [SerializeField] private NavMeshAgentHandler _movement;
    [SerializeField] private EnemyState _defaultState;
    [SerializeField] private Health _health;
    [SerializeField] private BubbleTrigger _bubbleTrigger;
    
    [Inject, UsedImplicitly] public Player Player { get; }
    
    public Animator Animator => _animatorLinker.Animator;
    public NavMeshAgentHandler Movement => _movement;
    public EnemyState DefaultState => _defaultState;
    public Vector3 TargetPostion => Player.transform.position;
    public Health Health => _health;
    public BubbleTrigger BubbleTrigger => _bubbleTrigger;


}
