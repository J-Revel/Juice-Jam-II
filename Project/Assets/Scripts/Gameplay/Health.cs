using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType 
{
    Normal,
    Blood,
}

public class Health : MonoBehaviour
{
    public Team team = Team.Enemy;
    public float health = 10f;
    public float changePerSecond = 0;
    public float maxHealth;
    private float initialMaxHealth;
    public GameObject deathFXPrefab;
    public System.Action hurtDelegate;
    public List<DamageType> immunities = new List<DamageType>();
    public System.Action deathDelegate;
    public float invincibilityDuration = 0;
    private float invincibilityTime;
    
    void Start()
    {
        if(maxHealth == 0)
            maxHealth = health;
        initialMaxHealth = maxHealth;
    }

    void OnDestroy()
    {
        
    }

    public bool OnDamageTaken(float damage)
    {
        if(invincibilityTime <= 0)
        {
            invincibilityTime = invincibilityDuration;
            health -= damage;
            if(health < 0)
            {
                health = 0;
                deathDelegate?.Invoke();
            }
            else
                hurtDelegate?.Invoke();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void TurnInvincible(float duration)
    {
        invincibilityTime = Mathf.Max(duration, invincibilityTime);
    }

    public void OnEaten()
    {
        StartCoroutine(EatenCoroutine());
    }

    private IEnumerator EatenCoroutine()
    {
        Instantiate(deathFXPrefab, transform.position, deathFXPrefab.transform.rotation);
        
        deathDelegate?.Invoke();
        Destroy(gameObject);
        yield return null;
    }

    void Update()
    {
        invincibilityTime -= Time.deltaTime;
        health += changePerSecond * Time.deltaTime;
        if(health > maxHealth)
        {
            health = maxHealth;
        }
    }
}
