using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class WeaponedRangedProjectile : Weapon
{
    [SerializeField, BoxGroup("Ranged")] private Transform _muzzle;

    public WeaponRangedProjectileData RangedData => Data as WeaponRangedProjectileData;

    protected override void Attack(Vector3 aimPosition, GameObject instigator, int team)
    {
        base.Attack(aimPosition, instigator, team);

        Debug.DrawLine(transform.position, aimPosition, Color.red, 1f);

        Vector3 spawnPos = _muzzle.position;
        Vector3 aimDir = (aimPosition - spawnPos).normalized;       // direction from A to B, is B minus A, normalized
        Quaternion spawnRot = Quaternion.LookRotation(aimDir);      // LookRotation() turns a DIRECTION into a ROTATION

        for (int i = 0; i < RangedData.ShotCount; i++)
        {
            // randomly generate inaccuracy
            float inaccX = Random.Range(-RangedData.Inaccuracy, RangedData.Inaccuracy);
            float inaccY = Random.Range(-RangedData.Inaccuracy, RangedData.Inaccuracy);

            // create rotation from inaccuracy (more Quaternion fun)
            Vector3 leftRightAngle = _muzzle.up * inaccX;
            Vector3 upDownAngle = _muzzle.right * inaccY;
            Quaternion inaccRotation = Quaternion.Euler(leftRightAngle + upDownAngle);  // Euler() turns euler angles (x,y,z) into a ROTATION

            // combine spawn rotation and inaccuracy rotation (we multiply quaternions to add their rotations)
            Quaternion finalRotation = spawnRot * inaccRotation;

            // spawn projectile and assign values
            Projectile spawnedProjectile = Instantiate(RangedData.BulletPrefab, spawnPos, finalRotation);
            spawnedProjectile.Launch(RangedData.ProjectileSpeed, RangedData.Range, RangedData.Damage, RangedData.DamageType, instigator, team);
        }
    }
}