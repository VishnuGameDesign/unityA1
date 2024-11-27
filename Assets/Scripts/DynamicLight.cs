using UnityEngine;

[RequireComponent (typeof(Light))]
public class DynamicLight : MonoBehaviour, IDamageable, IInteractable
{
    [SerializeField] private Light _light;

    public bool IsAlive => _light.enabled;

    public string InteractMessage => "Toggle Light";

    private void OnValidate()
    {
        _light = GetComponent<Light>();
    }

    public void Damage(DamageInfo damageInfo)
    {
        _light.enabled = false;
    }

    public void Interact(GameObject interactor)
    {
        _light.enabled = !_light.enabled;
    }
}