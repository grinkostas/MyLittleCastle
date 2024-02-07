using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InitializedProperty<T>
{
    private T _value;
    private bool _initialized = false;
    private Func<T> _constructor;
    
    public T Value
    {
        get
        {
            if (_initialized == false)
                _value = _constructor();
            _initialized = true;
            return _value;
        }
    }

    public InitializedProperty(Func<T> constructor)
    {
        _constructor = constructor;
    }

}
