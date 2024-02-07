using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using NaughtyAttributes;

public class BearTransformation : MonoBehaviour
{
    
    [SerializeField] private Transform _startModel;
    [SerializeField] private Transform _startRotateModel;
    [SerializeField] private Transform _endModel;
    [SerializeField] private CinemachineVirtualCamera _endModelCamera;
    [SerializeField] private float _zoomTime;
    [SerializeField] private ParticleSystem _poofParticle;
    [SerializeField] private ForwardMover _forwardMover;
    [SerializeField] private float _startMoveDelay;
    [SerializeField] private AnimatorLinker _animatorLinker;
    [SerializeField] private string _speedParam;
    [SerializeField] private float _speedParamValue;
    [SerializeField] private Vector3 _particleSpawnOffset;

    [Button()]
    private void Transform()
    {
        var startPosition = _startModel.transform.position;
        var startRotation = _startRotateModel.transform.rotation;
        _startModel.gameObject.SetActive(false);
        _endModel.transform.position = startPosition;
        _endModel.transform.rotation = startRotation;
        _endModel.localScale = Vector3.zero;
        _endModel.gameObject.SetActive(true);
        _endModel.DOScale(Vector3.one, _zoomTime).SetEase(Ease.OutBack);
        _poofParticle.transform.position = startPosition + _particleSpawnOffset;
        _poofParticle.Play();
        DOVirtual.DelayedCall(_startMoveDelay, () =>
        {
            _forwardMover.StartMover();
            _animatorLinker.Animator.SetFloat(_speedParam, _speedParamValue);
        });
        _endModelCamera.gameObject.SetActive(true);
    }
}
