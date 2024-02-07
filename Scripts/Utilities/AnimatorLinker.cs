using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimatorLinker : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public Animator Animator => _animator;

    public void SetBool(string parameterName, bool value)
    {
        _animator.SetBool(parameterName, value);
    }

    public void SetFloat(string parameterName, float value)
    {
        _animator.SetFloat(parameterName, value);
    }

    public void SetTrigger(string parameterName)
    {
        _animator.SetTrigger(parameterName);
    }
}
