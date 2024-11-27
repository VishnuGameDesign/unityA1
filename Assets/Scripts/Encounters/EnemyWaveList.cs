using UnityEngine;

[CreateAssetMenu]
public class EnemyWaveList : ScriptableObject
{
    [field: SerializeField] public EnemyWave[] Waves { get; private set; }
}