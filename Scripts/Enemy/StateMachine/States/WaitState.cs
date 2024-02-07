using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using StaserSDK.Utilities;
using Zenject;

public class WaitState : EnemyState
{
    [SerializeField] private ParticleSystem _sleepParticle;
    [SerializeField] private float _sleepParticleEnableDelay = 1.5f;
    [SerializeField] private Transform _model;
    [SerializeField] private float _reactionTime;
    [SerializeField] private bool _staticSpot;
    [SerializeField] private string _targetId = "";
    [SerializeField, ShowIf(nameof(_staticSpot))]
    private EnemySpot _enemySpot;

    [SerializeField, ShowIf(nameof(HaveAnimator)), AnimatorParam(nameof(Animator))] 
    private string _sleepParameter;
    
    [Inject] private Timer _timer;
    [Inject] private SpotManager _spotManager;
    
    private EnemySpot _currentSpot;

    private void OnDisable()
    {
        DOTween.Kill(_sleepParticle);
        _sleepParticle.Stop();
    }
    protected override void OnEnter()
    {
        if (_staticSpot)
        {
            _currentSpot = _enemySpot;
            _enemySpot.Use(Enemy);
        }

        DOVirtual.DelayedCall(_sleepParticleEnableDelay, Play).SetId(_sleepParticle);
        _model.gameObject.SetActive(true);
        Enemy.Movement.OnStopMove += OnReceiveDestination;
        MoveToSpot();
        Enemy.BubbleTrigger.OnEnterBubble.On(OnBubbleStartScaling);
    }

    protected override void OnExit()
    {
        DOTween.Kill(_sleepParticle);
        _sleepParticle.Stop();
        if (_staticSpot == false)
        {
            if(_currentSpot != null)
                _currentSpot.Leave(Enemy);
            _currentSpot = null;
        }
        
        Enemy.Movement.OnStopMove -= OnReceiveDestination;
        
        Enemy.BubbleTrigger.OnEnterBubble.Off(OnBubbleStartScaling);
        Enemy.Movement.StopMove();
        Animator.SetBool(_sleepParameter, false);
    }

    private void OnReceiveDestination()
    {
        UseWaitAnimation();
    }

    private int _attempts = 0;
    
    private void MoveToSpot()
    {
        DOTween.Kill(_sleepParticle);
        DOTween.Kill(this);
        _sleepParticle.Stop();

        if (_staticSpot)
        {
            Enemy.Movement.SetDestination(_currentSpot.transform.position);
            return;
        }
        if(_currentSpot != null)
            _currentSpot.BubbleTrigger.OnEnterBubble.Off(OnSpotEnterBubble);

        
        if (_targetId != "")
            _currentSpot = _spotManager.GetSpot(Enemy, _targetId);
        else
            _currentSpot = _spotManager.GetSpot(Enemy);
        
        if (_currentSpot == null)
        {
            if (_spotManager.HaveAvailableSpots(_targetId) == false)
            {
                Enemy.gameObject.SetActive(false);
                return;
            }
            
            _attempts++;
            if (_attempts > 3)
            {
                Enemy.gameObject.SetActive(false);
                return;
            }

            DOVirtual.DelayedCall(_reactionTime, MoveToSpot).SetId(this);
            return;
        }
        
        _currentSpot.BubbleTrigger.OnEnterBubble.On(OnSpotEnterBubble);
        if(Enemy.Movement.enabled && Enemy.Movement.gameObject.activeInHierarchy)
            Enemy.Movement.SetDestination(_currentSpot.transform.position);
        _attempts = 0;
        _currentSpot.Use(Enemy);
    }

    private void OnSpotEnterBubble(BubbleZone bubbleZone)
    {
        if(_currentSpot != null)
            _currentSpot.BubbleTrigger.OnEnterBubble.Off(OnSpotEnterBubble);
        MoveToSpot();
    }

    private void OnBubbleStartScaling(BubbleZone _)
    {
        if (_currentSpot.Inside)
        {
            _timer.ExecuteWithDelay(MoveToSpot, _reactionTime);
        }
    }

    private void Play()
    {
        if (enabled == false)
        {
            _sleepParticle.Stop();
            return;
        }

        _sleepParticle.Play();
    }
    private void UseWaitAnimation()
    {
        DOTween.Kill(_sleepParameter);
        Animator.SetBool(_sleepParameter, true);
        DOVirtual.DelayedCall(_sleepParticleEnableDelay, Play).SetId(_sleepParticle);
    }
}
