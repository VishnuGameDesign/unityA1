using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;    // namespace for all Odin Inspector stuff
using UnityEngine.Events;       // namespace for Unity events

// hides visible script name in inspector
[HideMonoScript]
public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private string _deathLayerName = "Corpse";
    [SerializeField, BoxGroup("Stats")] private float _current = 100f;
    [SerializeField, BoxGroup("Stats")] private float _max = 100f;

    // useful properties
    // => returns the calculation on the right side
    [BoxGroup("Debug"), ShowInInspector] public float Percentage => _current / _max;
    [BoxGroup("Debug"), ShowInInspector] public bool IsAlive => _current >= 1f;

    [FoldoutGroup("Events")] public UnityEvent<DamageInfo> OnDamage;
    [FoldoutGroup("Events")] public UnityEvent<DamageInfo> OnDeath;

    public void Damage(DamageInfo damageInfo)
    {
        if (!IsAlive) return;    // early return - function stops executing here
        if (damageInfo.Amount < 1f) return;

        // reduce health + clamp to avoid bad values
        _current -= damageInfo.Amount;
        _current = Mathf.Clamp(_current, 0f, _max);

        // damage event
        damageInfo.FinalPercentage = Percentage;
        OnDamage.Invoke(damageInfo);

        // TODO: add damage feedback


        // handle death
        if(!IsAlive)
        {
            gameObject.layer = LayerMask.NameToLayer(_deathLayerName);
            OnDeath.Invoke(damageInfo);
        }
    }

    [ContextMenu("Damage Test 10%"), Button("Damage Test 10%")]
    public void DamageTest()
    {
        DamageInfo damageInfo = new DamageInfo(_max * 0.1f, gameObject, gameObject, gameObject, DamageType.Bug);
        Damage(damageInfo);
    }
}

public class DamageInfo
{
    public DamageInfo(float amount, GameObject victim, GameObject source, GameObject instigator, DamageType damageType)
    {
        Amount = amount;
        Victim = victim;
        Source = source;
        Instigator = instigator;
        DamageType = damageType;
    }

    public float Amount { get; set; }
    public GameObject Victim { get; set; }          // person exploded
    public GameObject Source { get; set; }          // grenade
    public GameObject Instigator { get; set; }      // grenade thrower
    public DamageType DamageType { get; set; }
    public float FinalPercentage { get; set; }
}

public enum DamageType
{
    Normal,
    Fire,
    Ice,
    Lightning,
    Poison,
    Bug,
    Gun
}