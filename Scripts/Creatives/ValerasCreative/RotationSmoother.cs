using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using NaughtyAttributes;

public class RotationSmoother : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _percentDelta;
    [SerializeField] private bool _smooth;
    


    private SplinePositioner _splinePositioner = null;
    private Quaternion _currentRotation = Quaternion.identity;
    private Quaternion _targetRotation = Quaternion.identity;

    private void OnEnable()
    {
        _currentRotation = _player.Model.rotation;
    }

    private void OnDisable()
    {
        gameObject.SetActive(false);
        _splinePositioner = null;
    }

    private void LateUpdate()
    {
        Actualize();
    }

    private void OnValidate()
    {
        Actualize();
    }

    public void Enable(SplinePositioner positioner)
    {
        _splinePositioner = positioner;
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        //gameObject.SetActive(false);
        //_splinePositioner = null;
    }

    [Button("Actialize")]
    private void Actualize()
    {
        if(_splinePositioner == null)
            return;
        double currentPercent = _splinePositioner.GetPercent();
        if (currentPercent + _percentDelta > 1 || currentPercent + _percentDelta < 0)
        {
            Disable();
            return;
        }
        var sample = _splinePositioner.SampleCollection.Evaluate(currentPercent+_percentDelta);
        _targetRotation = Quaternion.LookRotation(sample.position-_player.transform.position, Vector3.up);
        _currentRotation = Quaternion.Lerp(_currentRotation, _targetRotation, Time.deltaTime * _rotateSpeed);
        _player.Model.rotation = _smooth ? _currentRotation : _targetRotation;
    }
}
