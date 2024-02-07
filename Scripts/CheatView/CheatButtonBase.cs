using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public abstract class CheatButtonBase : MonoBehaviour
{
    private Button _button;
    private Button Button
    {
        get
        {
            if (_button == null)
                _button = GetComponent<Button>();
            return _button;
        }
    }

    private void OnEnable()
    {
        Button.onClick.AddListener(OnButtonClicked);
    }

    private void OnDisable()
    {
        Button.onClick.RemoveListener(OnButtonClicked);
    }

    protected abstract void OnButtonClicked();
}
