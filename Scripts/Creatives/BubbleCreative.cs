using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class BubbleCreative : MonoBehaviour
{
    [SerializeField] private GameObject _shockWave;

    [Inject] private Bubble _bubble;

    private void OnEnable()
    {
        _bubble.ScaleChanged += OnScaled;
        _bubble.EndScale += OnEndScale;
    }

    private void OnDisable()
    {
        _bubble.ScaleChanged -= OnScaled;
        _bubble.EndScale += OnEndScale;
    }

    private void OnScaled(Bubble.ScaleData scaleData)
    {
        _shockWave.SetActive(scaleData.ShowCamera);
    }

    private void OnEndScale()
    {
        _shockWave.SetActive(true);
    }
}
