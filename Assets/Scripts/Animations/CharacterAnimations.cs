using EditorTools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimations : AutoMonoBehaviour
{
    // ALWAYS use SerializeField when getting components using AutoAssign or OnValidate
    [SerializeField, AutoAssign] private Animator _animator;
    [SerializeField, AutoAssign] private Rigidbody _rigidbody;
    [SerializeField, AutoAssign] private CustomCharacterMovement _movement;

    private void Start()
    {
        // set Death trigger when character dies, using OnDeath event from Health
        // one line example:
        // GetComponent<Health>().OnDeath.AddListener((DamageInfo info) => { _animator.SetTrigger("Death"); });
        // method example
        GetComponent<Health>().OnDeath.AddListener(Death);
    }

    private void Death(DamageInfo damageInfo)
    {
        _animator.SetTrigger("Death");
    }

    private void Update()
    {
        Vector3 worldVelocity = _rigidbody.linearVelocity;
        Vector3 localVelocity = transform.InverseTransformVector(worldVelocity);    // convert world space velocity to local velocity
        localVelocity /= _movement.Speed;   // normalize velocity to -1 to +1 range

        // set animator values
        _animator.SetFloat("MoveRight", localVelocity.x);
        _animator.SetFloat("MoveForward", localVelocity.z);
        _animator.SetBool("IsGrounded", _movement.IsGrounded);
    }
}