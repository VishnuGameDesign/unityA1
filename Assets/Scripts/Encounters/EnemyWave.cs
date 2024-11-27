using UnityEngine;

// System.Serializable allows this class to be viewed, edited, and saved in the inspector
[System.Serializable]
public class EnemyWave
{
    [field: SerializeField] public float InitialDelay { get; private set; } = 1f;        // delay before wave starts
    [field: SerializeField] public float SpawnDelay { get; private set; } = 0.5f;       // delay between each enemy spawn
    [field: SerializeField] public EnemyController[] EnemyPrefabs { get; private set; } // enemies to spawn (in order)
}