using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using PrimeTween;

public class BloodSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _decalPrefab;
    [SerializeField] private float _cooldown = 0.25f;
    [SerializeField] private float _duration = 10f;
    [SerializeField] private Vector2 _offsetRange = new Vector2(0.25f, 1f);
    [SerializeField] private Health _targetHealth;

    private float _lastSpawnTime;

    private void OnValidate()
    {
        if(_targetHealth == null)
        {
            _targetHealth = GetComponentInParent<Health>();
        }
    }

    private void Start()
    {
        _targetHealth.OnDamage.AddListener((DamageInfo DamageInfo) => { SpawnDecal(); });
    }

    // is called every time OnDamage invokes on Health component
    private void SpawnDecal()
    {
        // prevent decal spam
        if (Time.time < _lastSpawnTime + _cooldown) return;
        _lastSpawnTime = Time.time;

        float xAngle = 90f;
        float yAngle = Random.Range(0f, 360f);
        Vector3 eulerAngles = new Vector3(xAngle, yAngle, 0f);
        Quaternion rotation = Quaternion.Euler(eulerAngles);

        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
        Vector3 randomOffset = randomDirection * Random.Range(_offsetRange.x, _offsetRange.y);
        Vector3 position = _targetHealth.transform.position + randomOffset;

        GameObject decal = Instantiate(_decalPrefab, position, rotation);
        Destroy(decal, _duration);
    }
}