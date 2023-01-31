using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnDeath : MonoBehaviour
{
    private Health health;
    void Start()
    {
        health = GetComponent<Health>();
        health.deathDelegate += OnDeath;
    }
    
    void OnDeath()
    {
        Destroy(gameObject);
    }
}
