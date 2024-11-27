using UnityEngine;

[CreateAssetMenu(fileName = "WeaponMeleeData", menuName = "Data/Weapons/WeaponMeleeData")]
public class WeaponMeleeData : WeaponData
{
    [field: SerializeField] public MeleeComboData[] ComboData { get; private set; }
    [field: SerializeField] public LayerMask HitMask { get; private set; }
    [field: SerializeField] public float SpeedMultiplier { get; private set; } = 1f;
}

[System.Serializable]
public class MeleeComboData
{
    [field: SerializeField] public float DamageMultiplier { get; private set; } = 1f;
    [field: SerializeField] public float Angle { get; private set; } = 120f;
}