using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;

public class ResourceSeeker : MonoBehaviour
{
    [SerializeField] private List<ResourcePlace> _resourcePlaces;
    [SerializeField] private PlayerSplineFollower _playerSplineFollower;
    [SerializeField] private BounceUpAnimation _bounceUpAnimation;
    [SerializeField] private float _startBounceDelay;
    [SerializeField] private float _dealDamageDelay;
    [SerializeField] private Collector _collector;
    [SerializeField] private float _collectorEnableDelay;
    [SerializeField] private Transform _treeRecycler;
    [SerializeField] private float _treeRecyclerEnableDelay;

    private void OnEnable()
    {
        _playerSplineFollower.EndedPath.On(OnEndedPath);
    }

    [Button]
    private void OnEndedPath()
    {
        _collector.gameObject.SetActive(false);
        _treeRecycler.gameObject.SetActive(false);
        for (int i = 0; i < _resourcePlaces.Count; i++)
        {
            var place = _resourcePlaces[i];
            DOVirtual.DelayedCall(_startBounceDelay * (i + 1), () =>
            {
                _bounceUpAnimation.Animate(place.Model);
                DOVirtual.DelayedCall(_dealDamageDelay, () =>
                {
                    place.ApplyDamage(4);
                });
            });
        }

        DOVirtual.DelayedCall(_collectorEnableDelay, () => _collector.gameObject.SetActive(true));
        DOVirtual.DelayedCall(_treeRecyclerEnableDelay, () => _treeRecycler.gameObject.SetActive(true));
    }
}
