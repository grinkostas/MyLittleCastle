using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Dreamteck.Splines;

public class ShipMovement : MonoBehaviour
{
    [SerializeField] private SplinePositioner _positioner;
    [SerializeField] private Ease _ease;
    [SerializeField] private float _duration;
    private void Start()
    {
        DOVirtual.Float(0.2f, 0.5f, _duration, value => _positioner.SetPercent(value)).SetEase(_ease);
    }
}
