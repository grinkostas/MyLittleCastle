using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using Unity.VisualScripting;
using Zenject;
using Timer = StaserSDK.Utilities.Timer;

public class WeaponFx : MonoBehaviour
{
    [SerializeField] private Weapon _weapon;
    [SerializeField] protected WeaponController _weaponController;
    [SerializeField] private Transform _weaponModel;
    [SerializeField] private float _zoomTime;
    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private float _trailEnableDelay = 0.05f;
    [SerializeField] private float _trailAdditionDisableTime = 0.05f;

    [Inject] private Timer _timer;
    
    private Tweener _scaleTweener = null;
    
    private void OnEnable()
    {
        _weapon.StartUsing += Show;
        _weapon.EndedUsing += Hide;
        _weaponController.ChangedActive += OnChangedActive;
        Hide(_weapon);
    }
    
    private void OnDisable()
    {
        _weapon.StartUsing -= Show;
        _weapon.EndedUsing -= Hide;
    }
    
    private void Show(Weapon weapon)
    {
        _scaleTweener?.Kill();
        
        _timer.ExecuteWithDelay(EnableTrail, _trailEnableDelay);
        _scaleTweener = _weaponModel.DOScale(Vector3.one, _zoomTime).SetEase(Ease.OutBack);
        
    }

    private void EnableTrail()
    {
        _trailRenderer.emitting = true;
        DOVirtual.DelayedCall(_trailAdditionDisableTime, () =>
        {
            DOTween.Kill(this);
            _trailRenderer.emitting = false;
            DOVirtual.DelayedCall(_trailAdditionDisableTime, EnableTrail).SetId(this);
        }).SetId(this);
    }
    
    private void Hide(Weapon weapon)
    {
        _scaleTweener?.Kill();
        
        DOTween.Kill(this);
        _trailRenderer.emitting = false;
        _scaleTweener = _weaponModel.DOScale(Vector3.zero, _zoomTime);
    }
    

    private void OnChangedActive(bool active)
    {
        if(active)
            return;
        Hide(_weapon);
                

    }
}
