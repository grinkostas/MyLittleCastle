using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerBubbleParticle : MonoBehaviour
{
    [SerializeField] private MonoPool<PlayerBubbleParticleItem> _particleItemsPool;
    [SerializeField] private Transform _player;
    [SerializeField] private float _itemsShowDelay;

    private Vector3 _previousParticlePosition = Vector3.zero;

    private void OnEnable()
    {
        foreach (Transform child in _particleItemsPool.Parent)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(ShowItem), 2, _itemsShowDelay);
    }

    private void ShowItem()
    {
        var playerPosition = _player.transform.position;
        if(playerPosition == _previousParticlePosition)
            return;
        _previousParticlePosition = playerPosition;
        var item = _particleItemsPool.Get();
        item.transform.position = playerPosition;
    }
}
