using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class ResourcesFxInstaller : MonoInstaller
{
    [SerializeField] private ResourcesFx _resourcesFx;

    public override void InstallBindings()
    {
        Container.Bind<ResourcesFx>().FromInstance(_resourcesFx);
    }
}
