using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;

public class Torch : BubbleListener
{
    [SerializeField] private View _torchView;
    [SerializeField] private bool _animatorLinker;
    [SerializeField, HideIf(nameof(_animatorLinker))] private Animator _animator;
    [SerializeField, ShowIf(nameof(_animatorLinker))] private AnimatorLinker _linker;

    private string _startHandleTorchParam = "StartHandleTorch";
    private string _stopHandleTorchParam = "StopHandleTorch";
    private Animator Animator => _animatorLinker ? _linker.Animator : _animator;
    
    protected override void OnInsideBubble()
    {
        _torchView.Hide();
        Animator.SetTrigger(_stopHandleTorchParam);
    }

    protected override void OnOutsideBubble()
    {
        _torchView.Show();
        Animator.SetTrigger(_startHandleTorchParam);
    }
}
