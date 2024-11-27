using GameEvents;
using System;
using TMPro;
using UnityEngine;

public class InteractTargetText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textField;
    [SerializeField] private IInteractableEventAsset _interactTargetChangedEvent;

    private void OnEnable()
    {
        _interactTargetChangedEvent.OnInvoked.AddListener(InteractTargetChanged);
    }

    private void InteractTargetChanged(IInteractable interactable)
    {
        if (interactable == null) _textField.text = "";
        else _textField.text = interactable.InteractMessage;
    }

    private void OnDisable()
    {
        _interactTargetChangedEvent.OnInvoked.RemoveListener(InteractTargetChanged);
    }
}