using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class HealthComponent : MonoBehaviour
{
    public int maxHealth = 600;
    public int currentHealth;
     [SerializeField] public Slider healthBar;

    public int healthDecayMultiplier = 1; 
    public float healthDecayPerSecond = 2f; 

    void Start()
    {
        currentHealth = maxHealth;
        healthBar = GetComponent<Slider>();
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    void Update()
    {
        TakeDamage(Mathf.RoundToInt(healthDecayPerSecond * healthDecayMultiplier * Time.deltaTime));
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        healthBar.value = currentHealth; 
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;

        currentHealth = Mathf.Min(currentHealth, maxHealth);

        healthBar.value = currentHealth; 
    }

   
    public void Die()
    {
        Debug.Log("You died!"); 
        //Destroy(gameObject); 
    }
}
