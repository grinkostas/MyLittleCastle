using DG.Tweening;
using UnityEngine;
using Zenject;

namespace GameCore.Scripts.Creatives
{
    public class BubbleScalePresenter : MonoBehaviour
    {
        [SerializeField] private View _view;
        [SerializeField] private float _delay;
        
        [Inject] private Bubble _bubble;
        private void OnEnable()
        {
            _bubble.ScaleChanged += OnStartScaling;
        }

        private void OnDisable()
        {
            _bubble.ScaleChanged -= OnStartScaling;
        }

        private void OnStartScaling(Bubble.ScaleData targetScaleData)
        {
            float delay = 0;
            if (targetScaleData.ShowCamera)
                delay = _delay;
            DOVirtual.DelayedCall(delay, _view.Hide);
        }
    
    }
}