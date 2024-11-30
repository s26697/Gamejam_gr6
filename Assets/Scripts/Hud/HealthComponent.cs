using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class HealthComponent : MonoBehaviour
{
    [SerializeField] public int maxHealth = 600;
    public int currentHealth;
     [SerializeField] public Slider healthBar;

    public float healthDecayMultiplier = 1f; 
    public float healthDecayPerSecond = 2f; 
    private float  originalHealthDecayRate = 1;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar = GetComponent<Slider>();
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
        healthBar = GetComponent<Slider>();
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
