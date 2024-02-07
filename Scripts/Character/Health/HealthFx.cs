using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class HealthFx : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private ParticleSystem _damageParticle;
    [SerializeField] private Transform _model;
    [SerializeField] private Material _damageMaterial;
    [SerializeField] private float _damageFxDuration;
    [SerializeField] private bool _includeInactive;
    
    private Dictionary<Renderer, List<Material>> _defaultMaterialsData = new();
    private List<Renderer> _characterModels = new();
    private void Awake()
    {
        GetDefaultMaterials();
    }

    
    private void OnEnable()
    {
        _health.Damaged += OnDamaged;
    }
    
    private void OnDisable()
    {
        _health.Damaged -= OnDamaged;
    }
    
    private void GetDefaultMaterials()
    {
        _defaultMaterialsData.Clear();
        _characterModels = _model.GetComponentsInChildren<Renderer>(_includeInactive).ToList();
        foreach (var model in _characterModels)
        {
            var modelMaterials = new List<Material>(model.materials);
            _defaultMaterialsData.Add(model, modelMaterials);
        }
    }

    private void OnDamaged(int damage)
    {
        PlayParticle();
        SetDamagedMaterial();
        DOVirtual.DelayedCall(_damageFxDuration, SetDefaultMaterial);
    }

    private void PlayParticle()
    {
        _damageParticle.Stop();
        _damageParticle.time = 0;
        _damageParticle.Play();
    }

    private void SetDamagedMaterial()
    {
        foreach (var model in _characterModels)
        {
            model.materials = new[] { _damageMaterial };
        }
    }

    private void SetDefaultMaterial()
    {
        foreach (var defaultMaterialData in _defaultMaterialsData)
        {
            defaultMaterialData.Key.materials = defaultMaterialData.Value.ToArray();
        }
    }
}
