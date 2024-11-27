using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using Unity.VisualScripting;
using UnityEngine;

public class TowerDefenseEncounter : EncounterBase
{
    [SerializeField] private EnemyController[] _enemyPrefabs;
    [SerializeField] private List<Tower> _towers;
    private bool _allTowersDestroyed;

    public override void StartEncounter()
    {
        base.StartEncounter();
        
        ObjectiveTextEvent.Invoke("Destroy All Towers");
        StartCoroutine(EnemySpawnRoutine());
    }

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
    
    private void RemoveTower(GameObject towerObject)
    {
        _towers.Remove(_towers.Find(tower => tower.gameObject == towerObject));
    }
    
    private IEnumerator EnemySpawnRoutine()
    {
        while (!_allTowersDestroyed)
        {
            for (int i = 0; i < _enemyPrefabs.Length; i++)
            {
                SpawnEnemy(_enemyPrefabs[i]);
                yield return new WaitForSeconds(2f);
            }
            _allTowersDestroyed = AreAllTowersDestroyed();
            
            if(!_allTowersDestroyed) yield return new WaitForSeconds(5f);    
        }
        foreach (var enemy in _enemyPrefabs) Destroy(enemy.gameObject , 2f);
        ObjectiveTextEvent.Invoke("You Win!");
    }

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
