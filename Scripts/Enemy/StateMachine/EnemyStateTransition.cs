using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;

public abstract class EnemyStateTransition : MonoBehaviour
{
    [SerializeField] protected Enemy Enemy;
    [SerializeField] private bool _anySourceState;
    [SerializeField, HideIf(nameof(_anySourceState))] private EnemyState _sourceState;

    [SerializeField, ShowIf(nameof(_anySourceState))]
    private List<EnemyState> _possibleStates;
    
    [SerializeField] private EnemyState _targetState;

    protected bool Entered { get; private set; }
    
    private void OnEnable()
    {
        if (_anySourceState == false)
        {
            if (_sourceState.enabled)
            {
                StateEnter();
            }
            _sourceState.Entered += StateEnter;
            _sourceState.Exited += StateExit;
        }
        else
        {
            foreach (var possibleState in _possibleStates)
            {
                if(possibleState.enabled)
                    StateEnter();
                possibleState.Entered += StateEnter;
                possibleState.Exited += PossibleStateExit;
            }
        }
    }

    private void OnDisable()
    {
        if (_anySourceState)
        {
            foreach (var possibleState in _possibleStates)
            {
                possibleState.Entered -= StateEnter;
                possibleState.Exited -= PossibleStateExit;
            }
            return;
        }
        _sourceState.Entered -= StateEnter;
        _sourceState.Exited -= StateExit;
    }

    private void StateEnter()
    {
        if(Entered)
            return;
        Entered = true;
        OnStateEnter();
    }

    private void StateExit()
    {
        if(Entered == false)
            return;
        Entered = false;
        OnStateExited();
    }

    private void PossibleStateExit()
    {
        if(_possibleStates.Count(x=> x.enabled) > 0)
            return;
        StateExit();
    }
    
    protected abstract void OnStateEnter();

    protected abstract void OnStateExited();

    protected void Transit()
    {
        ExitSourceState();
        if(_targetState.enabled == false)
            _targetState.Enter();
    }

    protected void TransitToDefault()
    {
        ExitSourceState();
        if(_targetState.enabled)
            _targetState.Exit();
        Enemy.DefaultState.Enter();
    }

    private void ExitSourceState()
    {
        if(_anySourceState == false && _sourceState.enabled)
            _sourceState.Exit();

        if (_anySourceState)
        {
            foreach (var possibleState in _possibleStates)
            {
                if(possibleState.enabled)
                    possibleState.Exit();
            }
        }
    }
}
