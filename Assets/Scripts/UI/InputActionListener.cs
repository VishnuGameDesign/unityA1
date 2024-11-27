using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputActionListener : MonoBehaviour
{
    [SerializeField] private InputActionReference _actionReference;
    [SerializeField] private Button _activateButton;

    public UnityEvent OnInput;

    private void OnEnable()
    {
        // this is a C# event, rather than a UnityEvent
        // C# events don't have Add/Remove Listener functions, we use the += / -= convention to add or remove listeners
        _actionReference.action.performed += Performed;
    }

    private void OnDisable()
    {
        _actionReference.action.performed -= Performed;
    }

    private void Performed(InputAction.CallbackContext context)
    {
        OnInput.Invoke();
        // ?. is a built-in null check, if the object is null, we stop executing immediately
        _activateButton?.onClick.Invoke();
    }

    // allows us to manually invoke event without using input action
    public void ForceActivate()
    {
        OnInput.Invoke();
    }
}