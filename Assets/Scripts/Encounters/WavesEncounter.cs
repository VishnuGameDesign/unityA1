using System.Collections;
using UnityEngine;

public class WavesEncounter : EncounterBase
{
    [SerializeField] private EnemyWaveList _waveList;

    public override void StartEncounter()
    {
        base.StartEncounter();

        Debug.Log("Start waves encounter");

        StartCoroutine(WavesEncounterRoutine());
    }

    // I like to add Routine or State to the end of Coroutines so I don't accidentaly call them as normal functions
    private IEnumerator WavesEncounterRoutine()
    {
        ObjectiveTextEvent.Invoke("Zombies incoming!");

        for (int i = 0; i < _waveList.Waves.Length; i++)
        {
            // pause spawning while fighting enemies
            while(CurrentEnemies.Count > 0) yield return null;

            EnemyWave wave = _waveList.Waves[i];

            // wait for initial delay
            yield return new WaitForSeconds(wave.InitialDelay);

            // update objective text with current wave count
            string message = $"Remaining wave(s): {_waveList.Waves.Length - i}";
            ObjectiveTextEvent.Invoke(message);

            // spawn enemies from wave array
            for (int j = 0; j < wave.EnemyPrefabs.Length; j++)
            {
                SpawnEnemy(wave.EnemyPrefabs[j]);
                yield return new WaitForSeconds(wave.SpawnDelay);
            }
        }

        ObjectiveTextEvent.Invoke("Defeat remaining enemies.");

        // wait again for enemies to die
        while (CurrentEnemies.Count > 0) yield return null;

        FinishEncounter();
        ObjectiveTextEvent.Invoke("");
    }
}