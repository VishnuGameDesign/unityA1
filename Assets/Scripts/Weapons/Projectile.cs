using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent (typeof(CapsuleCollider))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    private float _speed;
    private float _range;
    private float _damage;
    private DamageType _damageType;
    private GameObject _insigator;
    private int _team;
    private Vector3 _spawnPosition;

    private void OnValidate()
    {
        if(_rigidbody == null)
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.useGravity = false;  // no bullet drop
            // enable more accurate collection detection for fast moving colliders
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            // enable rigidbody interpolation for smoother visual movement
            _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        }

        // try to get CapsuleCollider
        if(TryGetComponent(out CapsuleCollider capsuleCollider))
        {
            capsuleCollider.isTrigger = true;
        }
    }

    public void Launch(float speed, float range, float damage, DamageType damageType, GameObject instigator, int team)
    {
        _speed = speed;
        _range = range;
        _damage = damage;
        _damageType = damageType;
        _insigator = instigator;
        _team = team;

        // launch projectile
        _rigidbody.linearVelocity = transform.forward * speed;
        _spawnPosition = transform.position;
    }

    private void Update()
    {
        // clean up if range reached
        float distanceTraveled = Vector3.Distance(transform.position, _spawnPosition);
        if(distanceTraveled > _range)
        {
            Cleanup();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // ignore teammates
        if (other.TryGetComponent(out Targetable target) && target.Team == _team) return;

        // attempt to damage hit object
        if (other.TryGetComponent(out IDamageable damageable))
        {
            DamageInfo damageInfo = new DamageInfo(_damage, other.gameObject, gameObject, _insigator, _damageType);
            damageable.Damage(damageInfo);
        }

        Cleanup();
    }

    private void Cleanup()
    {
        Destroy(gameObject);

        // TODO: detach VFX trail
    }
}