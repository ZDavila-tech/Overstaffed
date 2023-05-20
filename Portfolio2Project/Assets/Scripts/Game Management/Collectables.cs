using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : MonoBehaviour
{
    [SerializeField] int value;
    /*enum CollectType 
    { 
      Health
    }

    [SerializeField] CollectType type;
    */

    private void OnTriggerEnter(Collider other)
    {
            if (other.CompareTag("Player"))
            {
                GameManager.instance.playerScript.TakeDamage(-value);
                Destroy(gameObject);
            }
    }
}
