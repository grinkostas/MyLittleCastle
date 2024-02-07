using Cinemachine;
using DG.Tweening;
using JetBrains.Annotations;
using StaserSDK;
using UnityEngine;
using Zenject;


public class NextSceneFx : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _endSceneCamera;
    
    [SerializeField] private float _uiShiwDelay;
    [SerializeField] private View _uiView;

    [Inject, UsedImplicitly] public Player Player { get; }
    
    public void Show()
    {
        _endSceneCamera.gameObject.SetActive(true);
        
        DOVirtual.DelayedCall(_uiShiwDelay, _uiView.Show);
        Player.Movement.DisableHandle(this);
    }
}
