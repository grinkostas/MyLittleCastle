using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Zenject;

public class TorchLightFade : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> _torchFireParticles;
    [SerializeField] private Light _light;
    [SerializeField] private OutsideTimer _outsideTimer;
    [SerializeField] private float _fadeInTime;
    [SerializeField] private View _torchFireView;
    [SerializeField] private float _lightFadeTime;
    [SerializeField] private float _minLightRange;
    
    private float _startRange = 5;

    private List<EmissionFader> _emissionFaders = new List<EmissionFader>();

    internal class EmissionFader
    {
        private ParticleSystem _particleSystem;
        private float _startOverLifetimeEmission = 8f;
        private float _startOverDistanceEmission = 1.5f;
        public EmissionFader(ParticleSystem particleSystem)
        {
            _particleSystem = particleSystem;
            
            var emission = _particleSystem.emission;
            _startOverDistanceEmission = emission.rateOverDistanceMultiplier;
            _startOverLifetimeEmission = emission.rateOverTimeMultiplier;
        }

        public void EvaluateProgress(float progress)
        {
            var emission = _particleSystem.emission;
            emission.rateOverTimeMultiplier = _startOverLifetimeEmission * progress;
            emission.rateOverDistanceMultiplier = _startOverDistanceEmission * progress;
        }
    }

    private void Awake()
    {
        _startRange = _light.range;
        foreach (var particle in _torchFireParticles)
            _emissionFaders.Add(new EmissionFader(particle));
        
    }

    private void OnEnable()
    {
        OnTimerProgressChanged(_outsideTimer.Progress);
        _outsideTimer.ProgressChanged += OnTimerProgressChanged;
    }
    
    private void OnDisable()
    {
        _outsideTimer.ProgressChanged -= OnTimerProgressChanged;
    }
    
    private void OnTimerProgressChanged(float progress)
    {
        float startProgress = progress;
        progress = 1 - progress;
        
        
        foreach (var emissionFader in _emissionFaders)
            emissionFader.EvaluateProgress(progress);
        
        if (progress <= 0.05f)
        {
            _torchFireView.Hide();
            FadeLight();
        }
        else
        {
            _light.range = (_startRange - ((_startRange - _minLightRange) * startProgress)) * _light.transform.localScale.y;
            _torchFireView.Show();
        }

    }

    private void FadeLight()
    {
        DOTween.To(() => _light.range, x => _light.range = x, 0, _lightFadeTime);
    }
}
