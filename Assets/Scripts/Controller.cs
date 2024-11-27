using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CustomCharacterMovement))]
[RequireComponent(typeof(Targetable))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Vision))]
// abstract prevents direct usage - we have to inherit from Controller first
// abstract classes can't be created using new () or added as components in Unity
public abstract class Controller : MonoBehaviour
{
    // { get; private set; } makes this a read-only property from outside this class
    // [field: SerializeField] is necessary to Serialize properties properly
    [field: SerializeField] public CustomCharacterMovement Movement { get; private set; }
    [field: SerializeField] public Targetable Targetable { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }
    [field: SerializeField] public Vision Vision { get; private set; }

    // inline buttons appear alongside the value in the inspector
    [field: SerializeField, InlineButton(nameof(FindWeapons), "Find")] public Weapon[] Weapons { get; private set; }

    // OnValidate is an EDITOR ONLY function - it gets called when anything in the inspector on this component changes
    // it also gets called when a component is first added
    protected virtual void OnValidate()
    {
        Movement = GetComponent<CustomCharacterMovement>();
        Targetable = GetComponent<Targetable>();
        Health = GetComponent<Health>();
        Vision = GetComponent<Vision>();
    }

    private void FindWeapons()
    {
        Weapons = GetComponentsInChildren<Weapon>();
    }
}