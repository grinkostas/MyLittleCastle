using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Utilities;
using Zenject;

public class BubbleIncreaser : MonoBehaviour
{
    [SerializeField] private BuyZone _buyZone;
    [SerializeField] private float _targetScale;
    [SerializeField] private bool _showCamera = true;

    [Inject] private Bubble _bubble;
    
    private void OnEnable()
    {
        if (_buyZone.IsBought())
        {
            OnBought();
            return;
        }
        _buyZone.Bought.On(OnBought);
    }

    private void OnDisable()
    {
        _buyZone.Bought.Off(OnBought);
    }

    private void OnBought()
    {
        _bubble.Increase(_targetScale, showCamera:_showCamera);
        _buyZone.Bought.Off(OnBought);
    }
}
