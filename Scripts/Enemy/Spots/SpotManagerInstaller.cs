using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class SpotManagerInstaller : MonoInstaller
{
    [SerializeField] private SpotManager _spotManager;
    public override void InstallBindings()
    {
        Container.Bind<SpotManager>().FromInstance(_spotManager);
    }
}
