using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponPickUp : MonoBehaviour
{
    [SerializeField] GameObject thisWeapon;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            equipWeapon(thisWeapon);
            Destroy(gameObject);
        }
    }

    public void equipWeapon(GameObject newWeapon)
    {
        GameObject newWeaponInstance = Instantiate(newWeapon);
        //gameManager.instance.playerScript.equipWeapon(newWeaponInstance);
    }
}
