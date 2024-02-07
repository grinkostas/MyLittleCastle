using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class CanvasInstaller : MonoInstaller
{
    [SerializeField] private MainCanvas _mainCanvas;
    [SerializeField] private OverlayCanvas _overlayCanvas;
    
    public override void InstallBindings()
    {
        Container.Bind<MainCanvas>().FromInstance(_mainCanvas);
        Container.Bind<OverlayCanvas>().FromInstance(_overlayCanvas);
    }
}
