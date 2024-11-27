using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class TimedEncounter : EncounterBase
{
    [SerializeField] private float _initialDelay = 1f;
    [SerializeField] private float _encounterDuration = 10f;
    [SerializeField] private float _enemySpawnDelay = 1f;
    [SerializeField] private float _enemySpawnDelayReduction = 0.05f;
    [SerializeField] private EnemyController[] _enemyPrefabs;

    public override void StartEncounter()
    {
        base.StartEncounter();

        StartCoroutine(TimedEncounterRoutine());
    }

    private IEnumerator TimedEncounterRoutine()
    {
        yield return new WaitForSeconds(_initialDelay);

        // initialize timers
        float encounterTimer = 0f;
        float spawnTimer = 0f;

        // spawn enemies while encounter runs
        while(encounterTimer < _encounterDuration)
        {
            encounterTimer += Time.deltaTime;
            spawnTimer += Time.deltaTime;

            // update objective timer
            // Remaining time: 6.0005865436s
            int remainingTime = Mathf.CeilToInt(_encounterDuration - encounterTimer);
            ObjectiveTextEvent.Invoke($"Time remaining: {remainingTime}");

            if(spawnTimer > _enemySpawnDelay)
            {
                // reset and shorten timer
                spawnTimer = 0f;
                _enemySpawnDelay *= (1f - _enemySpawnDelayReduction);

                // spawn random enemy
                int index = Random.Range(0, _enemyPrefabs.Length);
                SpawnEnemy(_enemyPrefabs[index]);
            }

            yield return null;
        }

        // wait for enemies to die
        ObjectiveTextEvent.Invoke("Defeat remaining enemies");
        while (CurrentEnemies.Count > 0) yield return null;

        FinishEncounter();
        ObjectiveTextEvent.Invoke("");
    }
}