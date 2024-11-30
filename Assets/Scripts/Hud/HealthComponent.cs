using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class HealthComponent : MonoBehaviour
{
    [SerializeField] public int maxHealth = 600;
     public float currentHealth;
     [SerializeField] public Slider healthBar;

    public float healthDecayMultiplier = 1f; 
    public float healthDecayPerSecond = 10f; 
    private float  originalHealthDecayRate;

    void Start()
    {
        Debug.Log("Initializing Health Component...");
        if (healthBar == null)
        {
            Debug.LogWarning("healthBar not assigned in Inspector. Attempting to find...");
            healthBar = GetComponent<Slider>();
        }

        if (healthBar == null)
        {
            Debug.LogError("Failed to find or assign healthBar. Please check the setup.", this);
        }
        else
        {
            Debug.Log("Health Bar successfully assigned.");
        }
        originalHealthDecayRate = healthDecayPerSecond;
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
        healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();
    }

    void Update()
    {
        TakeDamage((healthDecayPerSecond * healthDecayMultiplier * Time.deltaTime));
    }

    public void TakeDamage(float damage)
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

    public void DrinkGrease(GreaseComponent grease)
{
    StartCoroutine(ApplyGreaseEffect(grease));
}

    
    private IEnumerator ApplyGreaseEffect(GreaseComponent grease)
    {
        
        healthDecayPerSecond = originalHealthDecayRate * grease.decayMultiplier;
        Debug.Log("Grease applied: " + grease.name + ". Health decay reduced for " + grease.duration + " seconds.");

        yield return new WaitForSeconds(grease.duration);

        healthDecayPerSecond = originalHealthDecayRate;
    }
}
