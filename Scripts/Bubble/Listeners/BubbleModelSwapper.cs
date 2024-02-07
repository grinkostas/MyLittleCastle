using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BubbleModelSwapper : MonoBehaviour
{
    [SerializeField] private View _insideBubbleView;

    private List<BubbleZone> _enteredZones = new List<BubbleZone>();

    private void OnEnable()
    {
        if(_enteredZones.Count > 0)
            _insideBubbleView.Show();
        else
            _insideBubbleView.Hide();
    }

    private void OnDisable()
    {
        _enteredZones.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(enabled == false)
            return;
        
        if (other.TryGetComponent(out BubbleZone zone))
        {
            _enteredZones.Add(zone);
            _insideBubbleView.Show();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(enabled == false)
            return;
        if (other.TryGetComponent(out BubbleZone zone))
        {
            _enteredZones.Remove(zone);
            if(_enteredZones.Count == 0)
                _insideBubbleView.Hide();
        }
    }
}
