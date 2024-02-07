using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;
using NaughtyAttributes;
using Zenject;

public class HelperHouse : MonoBehaviour
{
    [SerializeField] private Transform _returnPoint;
    [SerializeField] private StackBase _houseStack;
    [SerializeField] private bool _searchFromAllResources = true;
    [SerializeField, HideIf(nameof(_searchFromAllResources))] private List<Transform> _resourcesParents;

    [Inject, UsedImplicitly] public ResourceController ResourceController { get; }
    public Vector3 ReturnPoint => _returnPoint.position;
    public StackBase Stack => _houseStack;

    private InitializedProperty<List<ResourcePlace>> _resourcePlacesProperty = null;

    public List<ResourcePlace> ResourcePlaces
    {
        get
        {
            if (_searchFromAllResources)
                return ResourceController.ResourcePlaces;
            if (_resourcePlacesProperty == null)
                _resourcePlacesProperty = new InitializedProperty<List<ResourcePlace>>(() =>
                {
                    List<ResourcePlace> places = new();
                    foreach (var parent in _resourcesParents)
                        places.AddRange(parent.GetComponentsInChildren<ResourcePlace>());
                    return places;
                });
            return _resourcePlacesProperty.Value;
        }
    }


}
