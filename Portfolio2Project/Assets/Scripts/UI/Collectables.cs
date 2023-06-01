using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : MonoBehaviour
{
    [SerializeField] int value;
    enum PickupType
    {
        Health,
        UltimateCharge
    }

    [SerializeField] PickupType Pickup;
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
            switch (Pickup)
            {
                case PickupType.Health:
                    GameManager.instance.playerController.TakeDamage(-value);
                    Destroy(gameObject);
                    break;
                case PickupType.UltimateCharge:
                    GameManager.instance.playerController.ChargeUt(value);
                    Destroy(gameObject);
                    break;
            }
        }
    }
}
