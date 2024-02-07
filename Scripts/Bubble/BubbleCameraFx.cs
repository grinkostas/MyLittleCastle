using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using StaserSDK.Utilities;
using Zenject;

public class BubbleCameraFx : MonoBehaviour
{
    [SerializeField] private Transform _cameraWrapper;
    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField] private float _showDelay;
    [SerializeField] private float _showTime;

    [Inject] private Bubble _bubble;

    private TimerDelay _showTimerDelay;
    private TimerDelay _hideTimerDelay;
    
    
    
    private void OnEnable()
    {
        _bubble.ScaleChanged += OnScaled;
    }

    private void OnDisable()
    {
        _bubble.ScaleChanged -= OnScaled;
    }
    
    private void Start()
    {
        Kill();
        Hide();
    }

    private void OnScaled(Bubble.ScaleData scaleData)
    {
        Kill();
        DOVirtual.DelayedCall(_showDelay + _showTime, Hide).SetId(_showTime);
        if(scaleData.ShowCamera == false)
            return;
        DOVirtual.DelayedCall(_showDelay, Show).SetId(this);
        _cameraWrapper.localScale = Vector3.one * scaleData.TargetScale;
    }

    private void Kill()
    {
        DOTween.Kill(_showDelay);
        DOTween.Kill(_showTime);
    }

    private void Show() => _camera.gameObject.SetActive(true);
    private void Hide() => _camera.gameObject.SetActive(false);
}
