using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpotManager : MonoBehaviour
{
    private List<EnemySpot> _enemySpots = new();

    public EnemySpot GetSpot(Enemy enemy)
    {
        return _enemySpots.FindAll(spot => spot.Busied == false && spot.Inside == false)
            .OrderBy(x => VectorExtentions.SqrDistance(enemy.transform, x.transform)).FirstOrDefault();
    }
    
    public EnemySpot GetSpot(Enemy enemy, string id)
    {
        return _enemySpots.FindAll(spot => spot.Id == id && spot.Busied == false && spot.Inside == false)
            .OrderBy(x => VectorExtentions.SqrDistance(enemy.transform, x.transform)).FirstOrDefault();
    }

    public bool HaveAvailableSpots(string id)
    {
        return _enemySpots.Where(x => x.Id == id).ToList().TrueForAll(x => x.BubbleTrigger.InsideBubble) == false;
    }

    public void Add(EnemySpot enemySpot)
    {
        if(_enemySpots.Contains(enemySpot))
            return;
        _enemySpots.Add(enemySpot);
    }

    public void Remove(EnemySpot enemySpot)
    {
        _enemySpots.Remove(enemySpot);
    }
}
