using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _duration = 3f;

    private void Start()
    {
        // detatch from parent bullet
        transform.SetParent(null);

        Destroy(gameObject, _duration);
    }

    private void Update()
    {
        // follow target while valid
        if(_target != null)
        {
            transform.position = _target.position;
        }
    }
}