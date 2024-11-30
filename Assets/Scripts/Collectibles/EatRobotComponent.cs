using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatRobotComponent : MonoBehaviour
{
    [SerializeField] public int maxHpIncrease = 10; 

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().EatRobot(this);
            Destroy(gameObject);
        }
    }
}
