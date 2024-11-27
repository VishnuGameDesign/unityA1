using PrimeTween;
using System;
using UnityEngine;

public class DamageScaleTween : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private ShakeSettings _scaleSettings;

    private void OnValidate()
    {
        if(_health == null) _health = GetComponentInParent<Health>();
    }

    private void OnEnable()
    {
        _health?.OnDamage.AddListener(Damaged);
    }

    private void OnDisable()
    {
        _health?.OnDamage.RemoveListener(Damaged);
    }

    private void Damaged(DamageInfo arg0)
    {
        Tween.PunchScale(transform, _scaleSettings);
    }
}