using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Random = UnityEngine.Random;

public class ModelRandomizer : MonoBehaviour
{
    [SerializeField] private List<GameObject> _models;
    [SerializeField] private bool _randomAllDisabled;

    private void OnEnable()
    {
        SelectRandomModel();
    }

    private bool TryGetRandomModel(out GameObject model)
    {
        model = null;
        int maxRandomValue = _models.Count + (_randomAllDisabled ? 1 : 0);
        int randomIndex = Random.Range(0, maxRandomValue);
        if (randomIndex == maxRandomValue - 1 && _randomAllDisabled)
            return false;
        model = _models[randomIndex];
        return true;
    }
    
    private void DisableAllModels()
    {
        foreach (var model in _models)
        {
            model.SetActive(false);
        }
    }
    
    private void SelectRandomModel()
    {
        GameObject targetModel;
        DisableAllModels();
        if (TryGetRandomModel(out targetModel) == false)
            return;
        targetModel.SetActive(true);
        
    }
}
