using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : MonoBehaviour
{
    [SerializeField] int value;
    enum PickupType
    {
        Health,
        UltimateCharge,
        Mimic,
        Exp
    }

    [SerializeField] PickupType Pickup;

    [SerializeField] GameObject mimic;
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
                    gameManager.instance.playerController.TakeDamage(-value);
                    Destroy(gameObject);
                    break;
                case PickupType.UltimateCharge:
                    gameManager.instance.playerController.ChargeUt(value);
                    Destroy(gameObject);
                    break;
                case PickupType.Mimic:
                    Instantiate(mimic, transform.position, mimic.transform.rotation);
                    LevelManager.instance.enemiesRemaining += 1;
                    Destroy(gameObject);
                    break;
                case PickupType.Exp:
                    gameManager.instance.playerStats.GainExp(value);
                    Destroy(gameObject);
                    break;
            }
        }
    }
}
