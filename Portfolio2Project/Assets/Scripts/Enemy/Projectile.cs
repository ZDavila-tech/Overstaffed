using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("----- Projectile Stats -----")]
    //the base damage this kind of shot has
    public int shotDmg;
    //the chance this shot has of being a critical hit (doubling the damage)
    [SerializeField] int critChance;
    //the amount of time this projectile takes to disappear (in seconds)
    [SerializeField] float bulletLife;
    //the speed that the projectile travels at
    [SerializeField] int bulletSpeed;

    [Header("----- Components -----")]
    //this object's Rigidbody
    [SerializeField] Rigidbody rb;
    GameObject player;
    Vector3 playerPosition;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Bullet Made");
        //destroy the bullet after it's lifespan ends
        Destroy(gameObject, bulletLife);
        //move the bullet
        player = GameObject.FindGameObjectWithTag("Player");
        playerPosition = player.transform.position;
        rb.velocity = (playerPosition - rb.position).normalized * bulletSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamage>() != null)
        {
            IDamage damageable = other.GetComponent<IDamage>();
            damageable.TakeDamage(shotDmg);
        }
        Destroy(gameObject);
    }
}
