using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] Renderer rModel;
    [SerializeField] NavMeshAgent navAgent;
    [Header("----- Enemy Stats -----")]
    [SerializeField] int iHP;
    [Header("----- Weapon Stats -----")]
    [Range(1, 100)][SerializeField] int iShootDistance;
    [Range(0, 2.5f)][SerializeField] float iShootRate;
    [SerializeField] GameObject gOBullet;
    bool bIsShooting;
    IDamage damageInterface;
    private Color cOrigColor;
    
    void Start()
    {
        cOrigColor = rModel.material.color;
    }

    
    void Update()
    {
<<<<<<< HEAD
        //navAgent.SetDestination(GameManager.instance.player.transform.position);//uses the GameManager to find the player and being moving towards them
=======
        navAgent.SetDestination(gameManager.instance.player.transform.position);//uses the GameManager to find the player and being moving towards them
>>>>>>> 120c50cbe3524e8f4ce8dceab6d5ef92b3b8763e
        if(!bIsShooting){//determines if the Shoot IEnumerator is currently active
            StartCoroutine(Shoot());//if not, begin shooting at the player
        }
    }
    
    IEnumerator Shoot(){
        bIsShooting = true;//tell update that this is running
        Instantiate(gOBullet, transform.position, transform.rotation);//create bullet
        
        yield return new WaitForSeconds(iShootRate);//cooldown
        bIsShooting = false;//tell update that we're ready to shoot again
    }

    public void TakeDamage(int dmg){
        iHP -= dmg;//health goes down
        StartCoroutine(flashColor());//indicate damage taken
        if(iHP <= 0){//if it dies, get rid of it
            Destroy(gameObject);
        }
    }

    IEnumerator flashColor(){//when it, change the color of the enemy from whatever it was to red, and back again
        rModel.material.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        rModel.material.color = cOrigColor;
    }
}
