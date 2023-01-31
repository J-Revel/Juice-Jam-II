using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HurtProceduralEffect : MonoBehaviour
{
    private Health health;
    private ProceduralAnimationHandler procAnimHandler;
    public ProceduralEffect effect;
    
    void Start()
    {
        health = GetComponentInParent<Health>();
        procAnimHandler = GetComponentInParent<ProceduralAnimationHandler>();
        health.hurtDelegate += OnHurt;
    }

    void OnHurt()
    {
        procAnimHandler.AddEffect(effect);
    }
}
