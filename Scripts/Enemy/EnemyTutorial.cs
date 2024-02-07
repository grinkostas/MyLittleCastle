using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyTutorial : MonoBehaviour
{
    [SerializeField] private Transform _pointerParent;
    [SerializeField] private Enemy _enemy;

    public Enemy Enemy => _enemy;
    public Health Health => _enemy.Health;
    public Transform PointerParent => _pointerParent;
}
