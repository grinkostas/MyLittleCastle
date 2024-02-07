using System;
using DG.Tweening;
using StaserSDK.Stack;
using UnityEngine;

namespace GameCore.Scripts.Stack
{
    public class StackItemLocatorPlacer : MonoBehaviour
    {
        [SerializeField] private StackBase _targetStack;
        [SerializeField] private StackItemLocator _stackItemLocator;
        [SerializeField] private float _zoomTime;

        private void OnEnable()
        {
            _targetStack.AddedItem += OnAddItem;
        }

        private void OnDisable()
        {
            _targetStack.AddedItem -= OnAddItem;
        }

        private void OnAddItem(StackItemData addData)
        {
            Transform target = addData.Target.transform;
            target.localScale = Vector3.zero;
            target.localPosition = _stackItemLocator.GetCurrentDelta();
            target.DOScale(Vector3.one, _zoomTime).SetEase(Ease.OutBack);
            _stackItemLocator.Rebuild();
        }
    }
}