using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using NaughtyAttributes;
using NepixSignals;
using UnityEngine.Rendering;

public class TransformationFx : MonoBehaviour
{
    [SerializeField] private Transform _startModel;
    [SerializeField] private Transform _startRotateModel;
    [SerializeField] private CinemachineVirtualCamera _startCamera;
    [SerializeField] private Transform _endModel;
    [SerializeField] private Transform _endRotateModel;
    [SerializeField] private CinemachineVirtualCamera _endCamera;
    [SerializeField] private float _changeModelsDelay;
    [SerializeField] private float _zoomTime;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _startModelFlyHeight;
    [SerializeField] private float _endModelFlyHeight;
    [SerializeField] private float _flyTime;
    [SerializeField] private float _rotateDuration;
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private ParticleSystem _poofParticle;
    [SerializeField] private ForwardMover _forwardMover;

    public TheSignal Started { get; } = new();
    
    
    private float _wastedTime = 0.0f;
    
    [Button]
    private void Transform()
    {
        _wastedTime = 0.0f;
        
        
        _startCamera.gameObject.SetActive(true);
        _endCamera.gameObject.SetActive(false);
        
        Vector3 startPosition = _startModel.transform.position;
        _particle.transform.position = startPosition;
        _particle.Play();
        _endModel.gameObject.SetActive(false);
        _endModel.transform.position = startPosition + Vector3.up * _endModelFlyHeight;
        _startModel.DOMove(_startModel.position + Vector3.up * _startModelFlyHeight, _flyTime);
        StartCoroutine(Rotate(_startRotateModel));

        _startModel.DOScale(Vector3.zero, _zoomTime).SetEase(Ease.InBack).OnComplete((() =>
        {
            _startCamera.gameObject.SetActive(false);
            _startModel.position = startPosition;
        }));
        DOVirtual.DelayedCall(_flyTime + _changeModelsDelay,() =>
        {
            _poofParticle.transform.position = _endModel.transform.position;
            _poofParticle.Play();
            _endModel.localScale = Vector3.zero;
            _endModel.gameObject.SetActive(true);
            Started.Dispatch();
            _endModel.rotation = _startModel.rotation;
            StartCoroutine(Rotate(_endRotateModel));
            _endCamera.gameObject.SetActive(false);
            _endCamera.gameObject.SetActive(true);
            _endModel.DOScale(Vector3.one, _zoomTime).SetEase(Ease.OutBack);
        });
        
        if(_forwardMover != null)
           _forwardMover.Stop();
        
        
        
    }

    private IEnumerator Rotate(Transform model)
    {
        while (_wastedTime < _rotateDuration)
        {
            _wastedTime += Time.deltaTime;
            model.rotation *= Quaternion.Euler((_rotateSpeed * Time.deltaTime) * Vector3.up);
            yield return null;
        }
    }
}
