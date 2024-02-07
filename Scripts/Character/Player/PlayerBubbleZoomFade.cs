using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class PlayerBubbleZoomFade : MonoBehaviour
{
    [SerializeField] private OutsideTimer _outsideTimer;
    [SerializeField] private Transform _playerBubble;
    [SerializeField] private float _startScale;
    [SerializeField] private float _minScale;
    [SerializeField] private float _zoomOutTime;

    private Tweener _zoomOutTweener;
    
    private void OnEnable()
    {
        OnTimerProgressChanged(_outsideTimer.Progress);
        _outsideTimer.ProgressChanged += OnTimerProgressChanged;
    }
    
    private void OnDisable()
    {
        _outsideTimer.ProgressChanged -= OnTimerProgressChanged;
        _zoomOutTweener = null;
    }

    private void OnTimerProgressChanged(float progress)
    {
        if (progress >= 0.98f)
        {
            if(_zoomOutTweener is null or { active: false })
                _zoomOutTweener = _playerBubble.transform.DOScale(Vector3.zero, _zoomOutTime).OnComplete(() => _zoomOutTweener = null);
            return;
        }
        float targetScale = _startScale - (_startScale - _minScale)  * progress;
        _playerBubble.transform.localScale = Vector3.one * targetScale;
        
    }
}
