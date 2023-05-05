using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("----- Projectile Stats -----")]
    //the base damage this kind of shot has
    [SerializeField] int shotDmg;
    //the chance this shot has of being a critical hit (doubling the damage)
    [SerializeField] int critChance;
    //the amount of time this projectile takes to disappear (in seconds)
    [SerializeField] int bulletLife;
    //the speed that the projectile travels at
    [SerializeField] int bulletSpeed;

    [Header("----- Components -----")]
    //this object's Rigidbody
    [SerializeField] Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        //destroy the bullet after it's lifespan ends
        Destroy(gameObject, bulletLife);
        //move the bullet
        rb.velocity = transform.forward * bulletSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
  
        IDamage damageable = other.GetComponent<IDamage>();
        if (damageable != null)
        {
            //check if critical change for this weapon is more than or equal the random generated number between 0 and 100 to see if it will or won't be a critial hit
            if(critChance >= Random.Range(0, 100)) //if it is
             damageable.TakeDamage(-(2*shotDmg));
            else
             damageable.TakeDamage(-shotDmg); //if it's not
        }

        Destroy(gameObject);
    }
}
