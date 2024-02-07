using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;

public class BirdWings : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer _renderer;
    [SerializeField] private float _loopDuration;
    [SerializeField] private float _returnDuration;
    
    [Button]
    private void StartLoop()
    {
        DOTween.Kill(this);
        var sequence = DOTween.Sequence();
        sequence.Append(BlendTween(0, 100, _loopDuration));
        sequence.Append(BlendTween(100, 0, _returnDuration));
        sequence.SetLoops(-1);
        sequence.SetId(this);
    }

    private Tween BlendTween(float start, float end, float duration)
    {
        return DOVirtual.Float(start, end, duration, value =>
        {
            _renderer.SetBlendShapeWeight(0, value);
        });
    }
    
    private void Start()
    {
        StartLoop();
    }
}
