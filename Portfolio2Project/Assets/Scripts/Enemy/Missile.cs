using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [Header("----- Missile Stats -----")]
    //the base damage this kind of shot has
    public int iDmg;
    //the chance this shot has of being a critical hit (doubling the damage)
    [SerializeField] int critChance;
    //the amount of time this projectile takes to disappear (in seconds)
    [SerializeField] float fMissileLife;
    //the speed that the projectile travels at
    [SerializeField] int iMissileSpeed;
    [SerializeField] float fRotateSpeed;

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
        Destroy(gameObject, fMissileLife);
        //move the bullet
        player = GameObject.FindGameObjectWithTag("Player");
        playerPosition = player.transform.position;
    }

    private void FixedUpdate() {
        rb.velocity = (playerPosition - rb.position).normalized * iMissileSpeed;
        RotateMissile();
    }

    private void RotateMissile(){
        var heading = transform.position;

        var rotation = Quaternion.LookRotation(heading);
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, fRotateSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamage>() != null)
        {
            IDamage damageable = other.GetComponent<IDamage>();
            damageable.TakeDamage(iDmg);
        }
        Destroy(gameObject);
    }
}
