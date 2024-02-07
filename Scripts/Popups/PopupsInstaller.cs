using UnityEngine;
using Zenject;

namespace GameCore.Scripts.Popups
{
    public class PopupsInstaller : MonoInstaller
    {
        [SerializeField] private CameraCanvas _cameraCanvas;
        [SerializeField] private OverlayCanvas _overlayCanvas;
        
        public override void InstallBindings()
        {
            Container.Bind<CameraCanvas>().FromInstance(_cameraCanvas);
            Container.Bind<OverlayCanvas>().FromInstance(_overlayCanvas);
            Container.BindInterfacesAndSelfTo<CanvasController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PopupFactory>().AsSingle().NonLazy();
        }
    }
}