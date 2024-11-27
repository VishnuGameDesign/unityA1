using UnityEngine;

public interface IDamageable
{
    // interfaces DO NOT SUPPORT FIELDS
    // bool IsAlive;

    // but they do support PROPERTIES
    // we don't add access modifier, everything in interfaces is public by default
    bool IsAlive { get; }

    // typically methods in interfaces don't have bodies, the classes that implement
    // the interface will have their own specific code
    void Damage(DamageInfo damageInfo);
}