using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponRangedProjectileData", menuName = "Data/Weapons/WeaponRangedProjectileData")]
public class WeaponRangedProjectileData : WeaponData
{
    [field: SerializeField, BoxGroup("Ranged Weapon")] public Projectile BulletPrefab { get; private set; }
    [field: SerializeField, BoxGroup("Ranged Weapon")] public int ShotCount { get; private set; } = 6;
    [field: SerializeField, BoxGroup("Ranged Weapon")] public float Inaccuracy { get; private set; } = 10f;
    [field: SerializeField, BoxGroup("Ranged Weapon")] public float ProjectileSpeed { get; private set; } = 30f;
}
