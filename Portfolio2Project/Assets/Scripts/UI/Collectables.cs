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
        Exp,
        Poison
    }

    [SerializeField] PickupType Pickup;
    [SerializeField] AudioSource collectSound;

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
                    if (value >= 0)
                        collectSound.PlayOneShot(AudioManager.instance.healthPickupAudio, AudioManager.instance.volumeScale);
                    else
                        collectSound.PlayOneShot(AudioManager.instance.hurtPickupAudio, AudioManager.instance.volumeScale);
                    gameManager.instance.playerController.TakeDamage(-value);
                    break;
                case PickupType.UltimateCharge:
                    gameManager.instance.playerController.ChargeUt(value);
                    break;
                case PickupType.Mimic:
                    Instantiate(mimic, transform.position, mimic.transform.rotation);
                    LevelManager.instance.enemiesRemaining += 1;
                    break;
                case PickupType.Exp:
                    gameManager.instance.playerStats.GainExp(value);
                    break;
                case PickupType.Poison:
                    gameManager.instance.playerController.Poison(value, 1);
                    break;
            }
            Destroy(gameObject);
        }
    }
}
