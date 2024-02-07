using NepixSignals;
using UnityEngine;


public abstract class ABuyZoneActivator : MonoBehaviour
{
    public abstract void DisableAll(bool disableZone = false);

    public abstract TheSignal Bought { get; }
    public abstract bool IsBought();
}
