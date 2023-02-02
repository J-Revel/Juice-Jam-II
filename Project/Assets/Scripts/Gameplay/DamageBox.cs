using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team {
    Player,
    Enemy,
}

public class DamageBox : MonoBehaviour
{
    public Team team;
    public StatEvaluator damage;
    
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Health otherHealth = other.GetComponentInParent<Health>();
        if(otherHealth != null && otherHealth.team != team)
        {
            otherHealth.OnDamageTaken(damage.value);
        }
    }

    void Update()
    {
        
    }
}
