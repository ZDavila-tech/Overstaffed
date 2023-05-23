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
    [SerializeField] Rigidbody _rb;
    GameObject goPlayer;
    Vector3 v3PlayerPos;


    // Start is called before the first frame update
    void Start()
    {
        //destroy the bullet after it's lifespan ends
        Destroy(gameObject, fMissileLife);
        //move the bullet
        goPlayer = gameManager.instance.player;
    }

    private void FixedUpdate() {
        v3PlayerPos = goPlayer.transform.position;
        _rb.velocity = (v3PlayerPos - _rb.position).normalized * iMissileSpeed;
        RotateMissile();
    }

    private void RotateMissile(){
        var heading = transform.position;

        var rotation = Quaternion.LookRotation(heading);
        _rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, fRotateSpeed * Time.deltaTime));
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
