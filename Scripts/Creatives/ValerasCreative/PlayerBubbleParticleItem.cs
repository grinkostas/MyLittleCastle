using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerBubbleParticleItem : MonoBehaviour, IPoolItem<PlayerBubbleParticleItem>
{
    public IPool<PlayerBubbleParticleItem> Pool { get; set; }
    public bool IsTook { get; set; }
    public void TakeItem()
    {
        gameObject.SetActive(true);
    }

    public void ReturnItem()
    {
        gameObject.SetActive(false);
    }
}
