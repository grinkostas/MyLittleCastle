using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class BubbleInstaller : MonoInstaller
{
    [SerializeField] private Bubble _bubble;

    public override void InstallBindings()
    {
        Container.Bind<Bubble>().FromInstance(_bubble);
    }
}
