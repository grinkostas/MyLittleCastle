using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CheatChangeActiveButton : MonoBehaviour
{
    [SerializeField] private List<GameObject> _attachedObjects;

    private bool _currentActive = true;
    
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
    
    private void OnButtonClicked()
    {
        _currentActive = !_currentActive;
        foreach (var attachedObject in _attachedObjects)
        {
            attachedObject.SetActive(_currentActive);
        }
    }
}
