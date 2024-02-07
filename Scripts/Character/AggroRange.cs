using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class AggroRange : MonoBehaviour
{
    [SerializeField] private OutsideTimer _outsideTimer;
    [SerializeField] private Destructible _health;

    public Destructible Health => _health;
    public OutsideTimer OutsideTimer => _outsideTimer;
}
