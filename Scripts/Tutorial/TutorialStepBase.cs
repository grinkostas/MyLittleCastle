using Cinemachine;
using DG.Tweening;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;
using Zenject;

public abstract class TutorialStepBase : MonoBehaviour
{
    [SerializeField] private string _id;
    [SerializeField] private bool _activeSelf;
    [SerializeField, HideIf(nameof(_lastStep))] private TutorialStepBase _nextStep;
    [SerializeField] private TutorialEventProvider _tutorialEvent;
    [SerializeField] private bool _lastStep;
    [SerializeField] private bool _additionalCamera;
    [SerializeField, ShowIf(nameof(_additionalCamera))] private CinemachineVirtualCamera _camera;
    [SerializeField, ShowIf(nameof(_additionalCamera))] private float _showDelay;
    [SerializeField, ShowIf(nameof(_additionalCamera))] private float _disableCameraDelay;

    [Inject] private TutorialPointer _tutorialPointer;

    
    public ITutorialEvent Event => _tutorialEvent.Interface;
    
    public abstract Transform Target { get; }
    
    public event UnityAction<TutorialStepBase> Entered;
    public event UnityAction<TutorialStepBase> Exited;
    public event UnityAction<TutorialStepBase> Ended;
    
    private void Start()
    {
        if(_additionalCamera)
            _camera.gameObject.SetActive(false);
        if(_activeSelf)
            Enter();
    }

    public void Enter()
    {
        if (ES3.Load(_id, false) || _tutorialEvent.Interface.IsFinished())
        {
            NextStep();
            return;
        }

        ForceEnter();
    }

    public void ForceEnter()
    {
        if(_tutorialEvent == null)
            return;
        
        ShowCamera(_showDelay);
        OnEnter();
        _tutorialEvent.Interface.Finished += NextStep;
        if (_tutorialEvent.Interface.IsAvailable())
        {
            ApplyTarget();
        }
        else
        {
            _tutorialEvent.Interface.Available += ApplyTarget;
        }

        Entered?.Invoke(this);
        SDKController.LevelStart(_id);
    
    }

    protected virtual void OnEnter()
    {
    }
    
    
    private void ApplyTarget()
    {
        _tutorialEvent.Interface.Available -= ApplyTarget;
        _tutorialPointer.SetTarget(Target);
    }

    private void NextStep()
    {
        Exit();
        if (_lastStep == false)
            _nextStep.Enter();
    }
    
    
    public void Exit(bool forced = false)
    {
        if (forced == false)
        {
            ES3.Save(_id, true);
            if (ES3.Load(_id, false) == false)
                SDKController.LevelComplete();
        } 

        OnExit();
        
        DOTween.Kill(this);
        if (_additionalCamera)
            _camera.gameObject.SetActive(false);

        if(forced == false)
            Ended?.Invoke(this);
        
        Exited?.Invoke(this);
        _tutorialEvent.Interface.Finished -= NextStep;
        _tutorialPointer.ReceiveDestination();
    }

    protected virtual void OnExit()
    {
        
    }

    public void ShowCamera()
    {
        ShowCamera(0);
    }

    public void ShowCamera(float delay)
    {
        if (_additionalCamera == false)
            return;
        
        DOTween.Kill(this);
        DOVirtual.DelayedCall(_showDelay, () => _camera.gameObject.SetActive(true)).SetId(this);
        DOVirtual.DelayedCall(_showDelay+_disableCameraDelay, () => _camera.gameObject.SetActive(false)).SetId(this);
    }
}
