using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [Header("----- Missile Stats -----")]
    public int damage; //the base damage this kind of shot has
    [SerializeField] float missileLife; //the amount of time this projectile takes to disappear (in seconds)
    [SerializeField] int missileSpeed; //the speed that the projectile travels at

    [Header("----- Components -----")]

    [SerializeField] Rigidbody rigidBody; //this object's Rigidbody
    GameObject player;
    Vector3 playerPosition;
    int tracksDone;

    // Start is called before the first frame update
    void Start()
    {
        //destroy the bullet after it's lifespan ends
        Destroy(gameObject, missileLife);
        //move the bullet
        player = gameManager.instance.player;
        playerPosition = player.transform.position;
        rigidBody.velocity = (playerPosition - rigidBody.position).normalized * missileSpeed;
        tracksDone = 0;
    }

    private void Update()
    {
        if (tracksDone < 2)
        {
            StartCoroutine(TrackPlayer());
        }
    }

    IEnumerator TrackPlayer()
    {
        yield return new WaitForSeconds(2);
        playerPosition = player.transform.position;
        rigidBody.velocity = (playerPosition - rigidBody.position).normalized * missileSpeed;
        ++tracksDone;
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

    //private void FixedUpdate() {
    //    v3PlayerPos = player.transform.position;
    //    if(Vector3.Distance(v3PlayerPos, _rb.position) > 1 && !bStopTracking){
    //        _rb.velocity = (v3PlayerPos - _rb.position).normalized * iMissileSpeed;
    //        RotateMissile();
    //        bStopTracking = true;
    //    }        
    //}


    //private void RotateMissile(){
    //    var heading = transform.position;

    //    var rotation = Quaternion.LookRotation(heading);
    //    _rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, fRotateSpeed * Time.deltaTime));
    //}
}
