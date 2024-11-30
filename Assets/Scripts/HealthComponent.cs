using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class HealthComponent : MonoBehaviour
{
    [SerializeField] public int maxHealth = 600;
     public float currentHealth;
     [SerializeField] public Slider healthBar;
     [SerializeField] public Slider OilBarSlider;
     private GameObject OilBar;

    public float healthDecayMultiplier = 1f; 
    public float healthDecayPerSecond = 10f; 
    private float  originalHealthDecayRate;

    void Start()
    {
        healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();
        OilBarSlider= GameObject.Find("OilBar").GetComponent<Slider>();
        OilBar = GameObject.Find("OilBar");
        originalHealthDecayRate = healthDecayPerSecond;
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
        OilBar.SetActive(false);
    }

    void Update()
    {
        TakeDamage((healthDecayPerSecond * healthDecayMultiplier * Time.deltaTime));
        DecreaseOilCount();
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
        OilBarSlider.maxValue = grease.duration;
        OilBarSlider.value = OilBarSlider.maxValue;
        OilBar.SetActive(true);
        yield return new WaitForSeconds(grease.duration);
        OilBar.SetActive(false);
        healthDecayPerSecond = originalHealthDecayRate;
    }

    public void HealthAdd(EatRobotComponent robot)
    {
        maxHealth += robot.maxHpIncrease * maxHealth / 100;
    }
    
    public void DecreaseOilCount()
    {
        if (OilBarSlider.value <= 0)
        {
            OilBarSlider.value = 0;
        }
        OilBarSlider.value -= 1 * Time.deltaTime;
    }
    
}
