using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GameCore.Scripts.Popups
{
    public class PopupFactory
    {
        [Inject, UsedImplicitly] public CanvasController CanvasController { get; }
        [Inject, UsedImplicitly] public DiContainer Container { get; }
        
        private List<PopupData> _spawnedPopups = new();
        public PopupSettings PopupSettings => PopupSettings.Value;

        public TPopup Get<TPopup>() where TPopup : APopup
        {
            if (IsAlreadySpawned<TPopup>())
                return (TPopup)_spawnedPopups.Find(x => x.Popup.Type == typeof(TPopup)).Popup;

            return SpawnPopup<TPopup>(PopupSettings.GetData<TPopup>());
        }

        public APopup Get(string id)
        {
            if (IsAlreadySpawned(id))
                return _spawnedPopups.Find(x => x.Id == id).Popup;
            
            var popup = SpawnPopup<APopup>(PopupSettings.GetData(id));
            var castedPopup = System.Convert.ChangeType(popup, popup.Type);
            return popup;
        }

        private TPopup SpawnPopup<TPopup>(PopupData popupData) where TPopup : APopup
        {
            if (popupData == null)
                throw new System.Exception($"Cannot find popup of type {typeof(TPopup)}");
            
            var popup = (TPopup)Object.Instantiate(popupData.Popup, CanvasController.Get(popupData.Popup.CanvasType).transform);
            Container.Inject(popup);
            Container.InjectGameObject(popup.gameObject);

            _spawnedPopups.Add(new PopupData(popupData.Id, popup));
            return popup;
        }

        public void Show<TPopup>(float delay = 0.0f) where TPopup : APopup
        {
            Get<TPopup>().Show(delay);
        }
        
        public void Show(string id, float delay = 0.0f)
        {
            Get(id).Show(delay);
        }
        
        public void Hide<TPopup>(float delay = 0.0f) where TPopup : APopup
        {
            if (IsAlreadySpawned<TPopup>() == false)
                return;
            
            Get<TPopup>().Hide(delay);
        }

        public bool IsAlreadySpawned<TPopup>()
        {
            return _spawnedPopups.Has(x => x.Popup.Type == typeof(TPopup));
        }
        
        public bool IsAlreadySpawned(string id)
        {
            return _spawnedPopups.Has(x => x.Id ==  id);
        }
    }
}