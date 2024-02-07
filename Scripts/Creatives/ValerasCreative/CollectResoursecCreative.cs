using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;

public class CollectResoursecCreative : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private GameObject _joystick;
    [SerializeField] private PlayerBubbleParticle _playerBubbleParticle;
    
    [Button()]
    private void Hide()
    {
        _player.Movement.gameObject.SetActive(false);
        _joystick.SetActive(false);
        _playerBubbleParticle.gameObject.SetActive(false);
    }

    [Button()]
    private void Show()
    {
        _player.Movement.gameObject.SetActive(false);
        _joystick.SetActive(false);
        _playerBubbleParticle.gameObject.SetActive(false);
    }
}
