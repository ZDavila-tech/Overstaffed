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
    [SerializeField] float cooldownBetweenTracks;

    [Header("----- Components -----")]
    [SerializeField] Rigidbody rigidBody; //this object's Rigidbody
    GameObject player;
    Vector3 playerPosition;
    int tracksDone;
    bool trackingCoolingDown;

    // Start is called before the first frame update
    void Start()
    {
        //destroy the bullet after it's lifespan ends
        Destroy(gameObject, missileLife);
        //move the bullet
        player = gameManager.instance.playerCharacter;
        playerPosition = player.transform.position;
        rigidBody.velocity = (playerPosition - rigidBody.position).normalized * missileSpeed;
        tracksDone = 0;
    }

    private void Update()
    {
        if (trackingCoolingDown == false && tracksDone < 5)
        {
            StartCoroutine(TrackPlayer());
        }
    }

    IEnumerator TrackPlayer()
    {
        trackingCoolingDown = true;
        playerPosition = player.transform.position;
        rigidBody.velocity = (playerPosition - rigidBody.position).normalized * missileSpeed;
        ++tracksDone;

        yield return new WaitForSeconds(cooldownBetweenTracks);

        trackingCoolingDown = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamage>() != null)
        {
            IDamage damageable = other.GetComponent<IDamage>();
            damageable.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
