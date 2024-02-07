using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;

public class CheatCameraPositionView : CheatCountView
{
    [SerializeField] private CinemachineVirtualCamera _playerCamera;
    [SerializeField] private Vector3 _changeAxis;
    [SerializeField] private float _divider = 4.0f;
    
    
    protected override void ActualizeValue(int value)
    {
        _countText.text = (value/_divider).ToString();
    }

    protected override void PlusButtonClick()
    {
        _playerCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset +=
            _changeAxis * (_step/_divider);
    }
    
    protected override void MinusButtonClick()
    {
        _playerCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset -=
            _changeAxis * (_step/_divider);
    }
    

}
