using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDefenseEncounter : EncounterBase
{
    [SerializeField] private EnemyController[] _enemyPrefabs;
    [SerializeField] private List<Tower> _towers;
    private bool _allTowersDestroyed;

    public override void StartEncounter()
    {
        base.StartEncounter();
        
        ObjectiveTextEvent.Invoke($"Destroy All Towers");
        StartCoroutine(EnemySpawnRoutine());
    }

    // subscribe to when towers are destroyed event to remove towers from the list 
    private void OnEnable()
    {
        foreach (var tower in _towers)
        {
            tower.OnTowerDestroyed.OnInvoked.AddListener(RemoveTower);
        }
    }
    
    private void OnDisable()
    {
        foreach (var tower in _towers)
        {
            tower.OnTowerDestroyed.OnInvoked.RemoveListener(RemoveTower);
        }
    }
    
    // find tower gameobject and remove from the list
    private void RemoveTower(GameObject towerObject)
    {
        _towers.Remove(_towers.Find(tower => tower.gameObject == towerObject));
    }
    
    private IEnumerator EnemySpawnRoutine()
    {
        // spawns enemy until all 3 towers are destroyed
        while (!_allTowersDestroyed)
        {
            for (int i = 0; i < _enemyPrefabs.Length; i++)
            {
                SpawnEnemy(_enemyPrefabs[i]);
                yield return new WaitForSeconds(2f);
            }
            _allTowersDestroyed = AreAllTowersDestroyed();
            
            // wait 5 seconds before the next wave
            if(!_allTowersDestroyed) yield return new WaitForSeconds(5f);    
        }
        
        ObjectiveTextEvent.Invoke($"Defeat remaining enemies. Enemies left: {CurrentEnemies.Count}");
        while (CurrentEnemies.Count > 0) yield return null;

        FinishEncounter();
        ObjectiveTextEvent.Invoke("All enemies killed. You Win!");
    }

    // check if all towers are destroyed 
    private bool AreAllTowersDestroyed()
    {
        if (_towers.Count <= 0)
        {
            _allTowersDestroyed = true;
            ObjectiveTextEvent.Invoke("All towers destroyed");
        }
        return _allTowersDestroyed;
    }
}
