using FMODUnity;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [field: SerializeField, Required, InlineEditor] public WeaponData Data { get; private set; }

    public float EffectiveRange => Data.EffectiveRange;
    public float Duration => Data.Duration;

    // TODO: add animation
    [field: SerializeField] public EventReference AttackSFX { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public string AnimationTrigger { get; private set; }

    private float _lastAttackTime;

    private void OnValidate()
    {
        if(Animator == null) Animator = GetComponentInParent<Animator>();
    }

    public bool TryAttack(Vector3 aimPosition, GameObject instigator, int team)
    {
        // throttle attack timing using cooldown
        float nextAttackTime = _lastAttackTime + 1f / Data.AttackRate;
        if(Time.time > nextAttackTime)
        {
            Attack(aimPosition, instigator, team);
            return true;
        }

        return false;
    }

    protected virtual void Attack(Vector3 aimPosition, GameObject instigator, int team)
    {
        // play FMOD SFXAW
        if (!AttackSFX.IsNull) RuntimeManager.PlayOneShot(AttackSFX, transform.position);

        // play animation if trigger is not null or empty
        if(!string.IsNullOrEmpty(AnimationTrigger)) Animator.SetTrigger(AnimationTrigger);
    }

    // optional override for attacks timed with animations
    public virtual void AttackAnimEvent(int attackIndex)
    {

    }
}