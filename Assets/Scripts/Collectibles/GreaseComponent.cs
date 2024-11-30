using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreaseComponent: MonoBehaviour
{
    [SerializeField] public float duration = 30f; 
    [SerializeField] public float decayMultiplier = 0.5f; 


void OnTriggerEnter2D(Collider2D other)
{
    if (other.gameObject.CompareTag("Player"))
    {
        other.gameObject.GetComponent<PlayerController>().UseGrease(this);
        Destroy(gameObject);
    }
}
}
