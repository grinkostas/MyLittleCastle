using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChangeOutlineColor : CheatButtonBase
{
    [SerializeField] private List<Outline> _outlines;
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _targetColor;

    private Color _currentColor = new Color();

    protected override void OnButtonClicked()
    {
        Color targetColor = _currentColor == _targetColor ? _defaultColor : _targetColor;

        foreach (var outline in _outlines)
            outline.OutlineColor = targetColor;
        
        _currentColor = targetColor;
    }
}
