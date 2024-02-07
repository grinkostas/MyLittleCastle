using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChangeOutlineActive : CheatButtonBase
{
    [SerializeField] private List<Outline> _outlines;

    private Outline.Mode _defaultMode = Outline.Mode.OutlineHidden;
    private Outline.Mode _targetMode = Outline.Mode.OutlineAll;
    private Outline.Mode _currentMode = Outline.Mode.OutlineHidden;
    
    protected override void OnButtonClicked()
    {
        Outline.Mode modeToSet = _currentMode == _defaultMode ? _targetMode : _defaultMode;
        foreach (var outline in _outlines)
        {
            outline.OutlineMode = modeToSet;
        }

        _currentMode = modeToSet;
    }
}
