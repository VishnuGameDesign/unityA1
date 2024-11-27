using EditorTools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PrimeTween;

public class HealthBar : AutoMonoBehaviour
{
    // AutoAssign automatically finds components when we add this script
    [SerializeField, AutoAssign(AutoAssignAttribute.AutoAssignMode.Parent)] private Health _health;
    [SerializeField] private Image _fastBar;
    [SerializeField] private Image _slowBar;
    [SerializeField] private float _lerpDuration = 0.25f;
    [SerializeField] private float _slowDelayTime = 0.25f;
    [SerializeField] private ShakeSettings _shakeSettings;

    private IEnumerator _fastLerpRoutine;
    private IEnumerator _slowLerpRoutine;

    private void Start()
    {
        _fastBar.fillAmount = 1f;
        _slowBar.fillAmount = 1f;
    }

    private void OnEnable()
    {
        _health?.OnDamage.AddListener(Damaged);
    }

    private void OnDisable()
    {
        _health?.OnDamage.RemoveListener(Damaged);
    }

    // Damaged is called every time OnDamage event on Health component is invoked
    private void Damaged(DamageInfo damageInfo)
    {
        // stop existing lerp routine
        if(_fastLerpRoutine != null) StopCoroutine(_fastLerpRoutine);
        _fastLerpRoutine = LerpBar(damageInfo.FinalPercentage, _lerpDuration, 0f, _fastBar);
        StartCoroutine(_fastLerpRoutine);

        if(_slowLerpRoutine != null) StopCoroutine(_slowLerpRoutine);
        _slowLerpRoutine = LerpBar(damageInfo.FinalPercentage, _lerpDuration, _slowDelayTime, _slowBar);
        StartCoroutine(_slowLerpRoutine);

        // animate shake using PrimeTween
        Tween.ShakeLocalPosition(transform, _shakeSettings);
    }

    private IEnumerator LerpBar(float targetFill, float duration, float delay, Image bar)
    {
        yield return new WaitForSeconds(delay);

        float currentFill = bar.fillAmount;
        float timer = 0f;
        while(timer < duration)
        {
            timer += Time.deltaTime;
            float progress = timer / duration;  // returns 0 to 1 progress value
            float fill = Mathf.Lerp(currentFill, targetFill, progress);
            bar.fillAmount = fill;

            yield return null;
        }
    }

    private void LateUpdate()
    {
        if (_health == null) return;

        // make health bar face camera
        transform.rotation = Camera.main.transform.rotation;
    }
}