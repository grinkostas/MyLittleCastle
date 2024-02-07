using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndPathPresenter : MonoBehaviour
{
    [SerializeField] private PlayerSplineFollower _playerSplineFollower;
    [SerializeField] private List<GameObject> _objectsToShow;
    [SerializeField] private List<GameObject> _objectsToHide;
    [SerializeField] private ZoomAnimation _zoomAnimation;

    private void OnEnable()
    {
        _playerSplineFollower.EndedPath.On(OnEndedPath);
        foreach (var objectToShow in _objectsToShow)
            objectToShow.SetActive(false);
    }

    private void OnEndedPath()
    {
        foreach (var objectToShow in _objectsToShow)
            objectToShow.SetActive(true);
        foreach (var objectToHide in _objectsToHide)
            _zoomAnimation.Animate(objectToHide.transform);
    }
}
