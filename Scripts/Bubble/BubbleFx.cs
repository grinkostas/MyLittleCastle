using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using StaserSDK.Utilities;
using Zenject;

public class BubbleFx : MonoBehaviour
{
    [SerializeField] private Bubble _bubble;
    
    [SerializeField] private Transform _bubbleWrapper;
    [SerializeField] private List<ParticleSystem> _particles;
    [SerializeField] private float _particlePlayDelay;

    private void OnEnable()
    {
        _bubble.ScaleChanged += OnStartScaling;
        _bubble.Scaled += OnScaled;
    }

    private void OnDisable()
    {
        _bubble.ScaleChanged -= OnStartScaling;
        _bubble.Scaled -= OnScaled;
    }

    private void OnStartScaling(Bubble.ScaleData targetScaleData)
    {
        if(targetScaleData.ShowCamera == false)
            return;
        DOVirtual.DelayedCall(_particlePlayDelay, ShowParticles);
    }
    
    private void OnScaled(float targetScale)
    {
        _bubbleWrapper.localScale = targetScale * Vector3.one;
    }
    private void ShowParticles()
    {
        foreach (var particle in _particles)
        {
            particle.Play();
        }
    }
}
