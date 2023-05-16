using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer rModel;
    [SerializeField] NavMeshAgent navAgent;
    [SerializeField] Transform tShootPos;
    [SerializeField] Transform tHeadPos;
    [SerializeField] GameObject drop;
    LevelManager lm;
    

    [Header("----- Enemy Stats -----")]
    [SerializeField] int iHP;
    [SerializeField] Slider hpBar;
    [SerializeField] GameObject hpDisplay;
    [SerializeField] float fTurnRate;
    [SerializeField] float fFieldOfView;
    [SerializeField] float fChaseTime;
    [Range(0, 100)][SerializeField] int DropRate;

    [Header("----- Weapon Stats -----")]
    [SerializeField] GameObject gOBullet;
    [Range(1, 100)][SerializeField] int iShootDistance;
    [Range(0, 2.5f)][SerializeField] float fShootRate;
    [SerializeField] float fShootAngle;

    bool bIsShooting;
    bool bPlayerInRange;
    bool bBeenShot;

    Vector3 playerDir;
    float fAngleToPlayer;

    IDamage damageInterface;
    private Color cOrigColor;
    
    void Start()
    {
        cOrigColor = rModel.material.color;
        ++gameManager.instance.enemiesRemaining;
        hpBar.maxValue = iHP;
        hpBar.value = hpBar.maxValue;
        lm = GetComponentInParent<LevelManager>();
    }


    void Update()
    {
        if (hpDisplay.activeSelf)
        {
            hpDisplay.transform.LookAt(gameManager.instance.player.transform.position);
        }
        if((bPlayerInRange || bBeenShot) && CanSeePlayer())
        {
            AttackPlayer();
        }
    }

    bool CanSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - tHeadPos.position;
        fAngleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);

        Debug.DrawRay(tHeadPos.position, playerDir);
        //Debug.Log(fAngleToPlayer);

        RaycastHit hit;
        if(Physics.Raycast(tHeadPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && fAngleToPlayer <= fFieldOfView)
            {

                return true;
            }
        }

        return false;
    }

    void FacePlayer()
    {

        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * fTurnRate);
    }

    void AttackPlayer()
    {
        navAgent.SetDestination(gameManager.instance.player.transform.position);
        if (navAgent.remainingDistance < navAgent.stoppingDistance)
        {
            //Debug.Log("YARGH");
            FacePlayer();
        }

        if (!bIsShooting && fAngleToPlayer <= fShootAngle)
        {
            StartCoroutine(Shoot());
        }
    }
    
    IEnumerator Shoot(){
        bIsShooting = true;//tell update that this is running
        Instantiate(gOBullet, transform.position, transform.rotation);//create bullet
        
        yield return new WaitForSeconds(fShootRate);//cooldown
        bIsShooting = false;//tell update that we're ready to shoot again
    }

    public void TakeDamage(int dmg)
    {
        iHP -= dmg;//health goes down
        StartCoroutine(showHealth());
        hpBar.value = iHP;
        StartCoroutine(flashColor());//indicate damage taken
        navAgent.SetDestination(gameManager.instance.player.transform.position);
        BeenShot();

        if(iHP <= 0) //if it dies, get rid of it
        {
            if(Random.Range(0,100) <= DropRate)
            {
             Instantiate(drop, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), transform.rotation);
            }
            --gameManager.instance.enemiesRemaining;
            lm.enemyKill();
            Destroy(gameObject);
        }
    }

    IEnumerator BeenShot()
    {
        bBeenShot = true;
        yield return new WaitForSeconds(fChaseTime);
        bBeenShot = false;
    }

    IEnumerator flashColor()
    {//when it, change the color of the enemy from whatever it was to red, and back again
        rModel.material.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        rModel.material.color = cOrigColor;
    }

    IEnumerator showHealth()
    {
        hpDisplay.SetActive(true);

        yield return new WaitForSeconds(2f);
        hpDisplay.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            bPlayerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bPlayerInRange = false;
        }
    }
}
