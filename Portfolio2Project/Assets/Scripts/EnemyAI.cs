using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyActions : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [Header("----- Enemy Stats -----")]
    [SerializeField] int healthPoints;
    [Header("----- Weapon Stats -----")]
    [Range(1, 100)][SerializeField] int shotDistance;
    [Range(0, 2.5f)][SerializeField] float shotRate;
    [SerializeField] GameObject bullet;
    bool isShooting;
    IDamage damageInterface;
    private Color colorOrg;
    
    void Start()
    {
        colorOrg = model.material.color;
    }

    
    void Update()
    {
        agent.SetDestination(GameManager.instance.player.transform.position);//uses the GameManager to find the player and being moving towards them
        if(!isShooting){//determines if the Shoot IEnumerator is currently active
            StartCoroutine(Shoot());//if not, begin shooting at the player
        }
    }
    
    IEnumerator Shoot(){
        isShooting = true;//tell update that this is running
        Instantiate(bullet, transform.position, transform.rotation);//create bullet
        
        yield return new WaitForSeconds(shotRate);//cooldown
        isShooting = false;//tell update that we're ready to shoot again
    }

    public void TakeDamage(int dmg){
        healthPoints -= dmg;//health goes down
        StartCoroutine(flashColor());//indicate damage taken
        if(healthPoints <= 0){//if it dies, get rid of it
            Destroy(gameObject);
        }
    }

    IEnumerator flashColor(){//when it, change the color of the enemy from whatever it was to red, and back again
        model.material.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        model.material.color = colorOrg;
    }
}
