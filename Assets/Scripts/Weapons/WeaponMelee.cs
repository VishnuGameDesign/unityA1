using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMelee : Weapon
{
    private Vector3 _aimPosition;
    private GameObject _instigator;
    private int _team;

    public WeaponMeleeData MeleeData => Data as WeaponMeleeData;

    protected override void Attack(Vector3 aimPosition, GameObject instigator, int team)
    {
        base.Attack(aimPosition, instigator, team);

        _aimPosition = aimPosition;
        _instigator = instigator;
        _team = team;

        Animator.SetFloat("MeleeSpeedMultiplier", MeleeData.SpeedMultiplier);
    }

    public override void AttackAnimEvent(int attackIndex)
    {
        base.AttackAnimEvent(attackIndex);

        MeleeComboData comboData = MeleeData.ComboData[attackIndex];

        Vector3 origin = _instigator.transform.position;
        Vector3 direction = (_aimPosition - origin).normalized;

        // WE'RE USING AN OVERLAPSHERE, NOT A SPHERECAST
        Collider[] hits = Physics.OverlapSphere(origin, Data.Range, MeleeData.HitMask);

        foreach (Collider hit in hits)
        {
            // don't punch self in face
            if (hit.gameObject == _instigator) continue;

            // optional ignore friendly fire
            // if (hit.TryGetComponent(out Targetable target) && target.Team == _team) continue;

            // filter hits by angle
            Vector3 targetDir = (hit.transform.position - origin).normalized;
            float angleToHit = Vector3.Angle(direction, targetDir);
            if (angleToHit > comboData.Angle) continue;

            // damage the target
            if (hit.TryGetComponent(out Health health))
            {
                float damage = Data.Damage * comboData.DamageMultiplier;
                DamageInfo damageInfo = new DamageInfo(damage, hit.gameObject, gameObject, _instigator, Data.DamageType);
                health.Damage(damageInfo);
            }
        }
    }
}