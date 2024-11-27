using GameEvents;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public abstract class EncounterBase : MonoBehaviour
{
    [field: SerializeField] public List<Waypoint> Waypoints { get; protected set; }
    [ShowInInspector] public List<EnemyController> CurrentEnemies { get; protected set; } = new List<EnemyController>();

    public UnityEvent OnEncounterStarted;
    public UnityEvent OnEncounterFinished;
    public StringEventAsset ObjectiveTextEvent;

    [Button]
    private void FindWaypoints()
    {
        // .ToList() can switch from array to list, and .ToArray() goes back again
        // this is expensive, so we don't want to do it in Update or anywhere else frequently
        Waypoints = GetComponentsInChildren<Waypoint>().ToList();
    }

    public virtual void StartEncounter()
    {
        OnEncounterStarted.Invoke();
    }

    protected virtual void FinishEncounter()
    {
        OnEncounterFinished.Invoke();
    }

    protected Transform GetRandomSpawnPoint()
    {
        int randomIndex = Random.Range(0, Waypoints.Count);
        return Waypoints[randomIndex].transform;
    }

    protected virtual void SpawnEnemy(EnemyController enemyPrefab)
    {
        Transform spawnPoint = GetRandomSpawnPoint();
        // instantiate enemy, it will return the EnemyController component automatically
        EnemyController spawned = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation, transform);

        // add to current list
        CurrentEnemies.Add(spawned);
        spawned.GetComponent<Health>().OnDeath.AddListener(OnEnemyKilled);
    }

    private void OnEnemyKilled(DamageInfo damageInfo)
    {
        // check for valid enemy
        EnemyController enemyController = damageInfo.Victim.GetComponent<EnemyController>();
        if(enemyController != null && CurrentEnemies.Contains(enemyController))
        {
            // remove enemy from list, and remove listener
            CurrentEnemies.Remove(enemyController);
            enemyController.GetComponent<Health>().OnDeath.RemoveListener(OnEnemyKilled);
        }
    }
}