using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    [field: SerializeField, BoxGroup("Weapon")] public float Damage { get; private set; } = 5f;
    [field: SerializeField, BoxGroup("Weapon")] public float Range { get; private set; } = 5f;
    [field: SerializeField, BoxGroup("Weapon")] public float EffectiveRange { get; private set; } = 4f;
    [field: SerializeField, BoxGroup("Weapon")] public float AttackRate { get; private set; } = 2f;
    [field: SerializeField, BoxGroup("Weapon")] public float Duration { get; private set; } = 1f;
    [field: SerializeField, BoxGroup("Weapon")] public DamageType DamageType { get; private set; } = DamageType.Normal;
}