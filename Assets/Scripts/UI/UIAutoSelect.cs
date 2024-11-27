using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIAutoSelect : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private bool _firstSelected = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    private void OnEnable()
    {
        if (_firstSelected) EventSystem.current.SetSelectedGameObject(gameObject);
    }

    private void Start()
    {
        if(_firstSelected) EventSystem.current.SetSelectedGameObject(gameObject);
    }
}