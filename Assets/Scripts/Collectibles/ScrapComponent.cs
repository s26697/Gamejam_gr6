using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapComponent : MonoBehaviour
{
    [SerializeField] public int HealValue = 50; // in percent

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().UseScrap(this);
            Destroy(gameObject);
        }
    }
}
