using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HurtProceduralEffect : MonoBehaviour
{
    private Health health;
    private ProceduralAnimationHandler procAnimHandler;
    public ProceduralEffect effect;
    public Transform impactPointFXPrefab;
    
    void Start()
    {
        health = GetComponentInParent<Health>();
        procAnimHandler = GetComponentInParent<ProceduralAnimationHandler>();
        health.hurtDelegate += OnHurt;
    }

    void OnHurt(Ray damageRay)
    {
        procAnimHandler.AddEffect(effect);
        if(impactPointFXPrefab != null)
            Instantiate(impactPointFXPrefab, damageRay.origin, Quaternion.LookRotation(damageRay.direction));
    }
}
