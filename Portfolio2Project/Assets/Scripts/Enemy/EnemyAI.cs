using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour, IDamage
{
    LevelManager levelManager;

    [Header("----- Components -----")]
    [SerializeField] Renderer rModel;
    [SerializeField] NavMeshAgent navAgent;
    [SerializeField] Transform tShootPos;
    [SerializeField] Transform tHeadPos;
    [SerializeField] GameObject drop;
    [SerializeField] Animator anim;    

    [Header("----- Enemy Stats -----")]
    [SerializeField] int iHP;
    [SerializeField] Slider hpBar;
    [SerializeField] GameObject hpDisplay;
    [SerializeField] float fTurnRate;
    [SerializeField] float fFieldOfView;
    [SerializeField] float fChaseTime;
    [Range(0, 100)][SerializeField] int DropRate;
    [SerializeField] float animTransSpeed;

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

    //IDamage damageInterface;
    private Color cOrigColor;
    
    void Start()
    {
        levelManager = LevelManager.instance;
        ++levelManager.enemiesRemaining;

        cOrigColor = rModel.material.color;
        
        hpBar.maxValue = iHP;
        hpBar.value = hpBar.maxValue;
    }


    void Update()
    {
        if (anim != null)
        {
            float speed = 0;
            speed = Mathf.Lerp(speed, navAgent.velocity.normalized.magnitude, Time.deltaTime * animTransSpeed);
            anim.SetFloat("Speed", speed);
        }


        if (hpDisplay.activeSelf)
        {
            hpDisplay.transform.LookAt(GameManager.instance.player.transform.position);
        }
        if((bPlayerInRange || bBeenShot) && CanSeePlayer())
        {
            AttackPlayer();
        }
    }

    bool CanSeePlayer()
    {
        playerDir = GameManager.instance.player.transform.position - tHeadPos.position;
        fAngleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);

        Debug.DrawRay(tHeadPos.position, playerDir);
        //Debug.Log(fAngleToPlayer);

        if(Physics.Raycast(tHeadPos.position, playerDir, out RaycastHit hit))
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
        navAgent.SetDestination(GameManager.instance.player.transform.position);
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
        anim.SetTrigger("Attack");//play the shooting animation

        yield return new WaitForSeconds(fShootRate);//cooldown
        bIsShooting = false;//tell update that we're ready to shoot again
    }

    public void TakeDamage(int dmg)
    {
        iHP -= dmg;//health goes down
        StartCoroutine(ShowHealth());
        hpBar.value = iHP;
        StartCoroutine(FlashColor());//indicate damage taken
        navAgent.SetDestination(GameManager.instance.player.transform.position);
        StartCoroutine(BeenShot());

        if(iHP <= 0) //if it dies, get rid of it
        {
            if(Random.Range(0,100) <= DropRate && drop != null)
            {
                Instantiate(drop, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), transform.rotation);
            }
            --levelManager.enemiesRemaining;
            Destroy(gameObject);
        }
    }

    IEnumerator BeenShot()
    {
        bBeenShot = true;
        yield return new WaitForSeconds(fChaseTime);
        bBeenShot = false;
    }

    IEnumerator FlashColor()
    {//when it, change the color of the enemy from whatever it was to red, and back again
        rModel.material.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        rModel.material.color = cOrigColor;
    }

    IEnumerator ShowHealth()
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
