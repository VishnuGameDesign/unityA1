using GameEvents;
using UnityEngine;

public class Tower : MonoBehaviour, IDamageable
{
    [field: SerializeField] private Targetable Targetable;
    [field: SerializeField] private Health Health;
    [SerializeField] public GameObjectEventAsset OnTowerDestroyed;
   
    public bool IsAlive { get; }

    private void OnValidate()
    {
        if(Targetable == null) Targetable = GetComponent<Targetable>();        
        if(Health == null) Health = GetComponent<Health>();        
    }

    // listen to health damage and death events 
    private void OnEnable()
    {
        Health.OnDamage.AddListener(Damage);
        Health.OnDeath.AddListener(DestroyGameObject);  
    }
    
    private void OnDisable()
    {
        Health.OnDamage.RemoveListener(Damage);
        Health.OnDeath.RemoveListener(DestroyGameObject);  
    }

    public void Damage(DamageInfo damageInfo) {} 
    
    // destroy the tower OnDeath
    private void DestroyGameObject(DamageInfo damageInfo)
    {
        OnTowerDestroyed?.Invoke(this.gameObject);
        Destroy(this.gameObject);
    }
}
