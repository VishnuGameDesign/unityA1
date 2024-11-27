using UnityEngine;

public interface IInteractable
{
    string InteractMessage { get; }
    void Interact(GameObject interactor);
}