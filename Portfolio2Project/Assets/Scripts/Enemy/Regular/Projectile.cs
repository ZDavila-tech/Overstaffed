using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("----- Projectile Stats -----")]
    //the base damage this kind of shot has
    public int shotDmg;
    //the amount of time this projectile takes to disappear (in seconds)
    [SerializeField] float bulletLife;
    //the speed that the projectile travels at
    [SerializeField] int bulletSpeed;

    float bulletVariance = 0;

    [Header("----- Components -----")]
    //this object's Rigidbody
    [SerializeField] Rigidbody rb;
    GameObject player;
    Vector3 playerPosition;

    enum Effect
    {
        Base,
        Poison,
        Burn,
        Freeze
    }

    [SerializeField] Effect effect;
    [SerializeField] float duration;
    [SerializeField] float timeBetween;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Bullet Made");
        //destroy the bullet after it's lifespan ends
        Destroy(gameObject, bulletLife);
        //move the bullet
        player = GameObject.FindGameObjectWithTag("Player");
        playerPosition = player.transform.position;
        Vector3 variance = new Vector3(Random.Range(-bulletVariance, bulletVariance), Random.Range(-bulletVariance, bulletVariance), 0);
        rb.velocity = ((playerPosition - rb.position).normalized * bulletSpeed);
        rb.velocity = variance + rb.velocity;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamage>() != null)
        {
            IDamage damageable = other.GetComponent<IDamage>();
            damageable.TakeDamage(shotDmg);
            switch (effect)
            {
                case Effect.Poison:
                    damageable.Poison(duration, timeBetween); break;
                case Effect.Burn:
                    damageable.Burn(duration, timeBetween); break;
                case Effect.Freeze:
                    damageable.Freeze(duration); break;
            }
        }
        Destroy(gameObject);
    }

    public void SetDamage(int damage)
    {
        shotDmg = damage;
    }

    public void SetVariance(float _variance)
    {
        bulletVariance= _variance;
    }
}
