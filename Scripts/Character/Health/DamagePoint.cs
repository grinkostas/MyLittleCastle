using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;

public class DamagePoint : MonoBehaviour, IPoolItem<DamagePoint>
{
    [SerializeField] private TMP_Text _damageText;
    [SerializeField] private List<View> _views;
    [SerializeField] private Vector3 _randomPositionAxis;
    [SerializeField] private Vector2 _randomPositionRange;
    [SerializeField] private int _multiplayer;
    [SerializeField] private float _returnDelay;
    
    public IPool<DamagePoint> Pool { get; set; }
    public bool IsTook { get; set; }
    
    public void SetDamage(int damage)
    {
        _damageText.text = (damage*_multiplayer).ToString();
    }
    
    public void TakeItem()
    {
        ResetPosition();
        foreach (var view in _views)
            view.Show();

        DOVirtual.DelayedCall(_returnDelay, () =>
        {
            foreach (var view in _views)
            {
                view.Hide();
                view.HideComplete.Once(() => Pool.Return(this));
            }
        });
    }

    public void ReturnItem()
    {
        gameObject.SetActive(false);
    }

    private void ResetPosition()
    {
        transform.localPosition = Vector3.zero + _randomPositionRange.Random() * _randomPositionAxis;
    }
}
