using UnityEngine;

public class Duplicator : MonoBehaviour, IInteractable
{
    public string InteractMessage => $"Duplicate: {gameObject.name}";

    public void Interact(GameObject interactor)
    {
        Instantiate(gameObject, transform.position + Random.onUnitSphere, Quaternion.identity);
    }
}