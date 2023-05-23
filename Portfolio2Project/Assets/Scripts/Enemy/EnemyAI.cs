using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour, IDamage, IPhysics
{
    LevelManager levelManager;

    [Header("----- Components -----")]
    [SerializeField] Renderer rModel;
    [SerializeField] NavMeshAgent navAgent;
    [SerializeField] Transform shootPosition;
    [SerializeField] Transform headPosition;
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
    [SerializeField] int chargeValue;

    [Header("----- Weapon Stats -----")]
    [SerializeField] GameObject bullet;
    [Range(1, 100)][SerializeField] int shootDistance;
    [Range(0, 2.5f)][SerializeField] float shootRate;
    [SerializeField] float shootAngle;

    bool bIsShooting;
    bool bPlayerInRange;
    bool bBeenShot;
    public bool isSlowed;

    Vector3 playerDir;
    float fAngleToPlayer;

    //IDamage damageInterface;
    private Color cOrigColor;
    
    void Start()
    {
        isSlowed = false;
        if(LevelManager.instance != null)
        {
            levelManager = LevelManager.instance;
        }

        cOrigColor = rModel.material.color;
        
        hpBar.maxValue = iHP;
        hpBar.value = hpBar.maxValue;
        if(anim !=  null)
        {
            anim.SetFloat("Shoot Rate", 1 / shootRate);
        }
    }


    void Update()
    {
        //StartCoroutine(SlowEnemy());
        if (anim != null)
        {
            float speed = 0;
            speed = Mathf.Lerp(speed, navAgent.velocity.normalized.magnitude, Time.deltaTime * animTransSpeed);
            anim.SetFloat("Speed", speed);
        }

        

        if (hpDisplay.activeSelf)
        {
            hpDisplay.transform.LookAt(gameManager.instance.player.transform.position);
        }
        if((bPlayerInRange || bBeenShot) && CanSeePlayer())
        {
            AttackPlayer();
        }
    }
    //IEnumerator SlowEnemy()
    //{
    //    if (isSlowed)
    //    {
    //        WaterSpecial wspec;
    //        wspec = new WaterSpecial();
    //        NewStaff st;
    //        st = new NewStaff();

    //        navAgent.speed /= wspec.slowRate;
    //        yield return new WaitForSeconds(st.slowDuration);
    //        navAgent.speed *= wspec.slowRate;
    //    }
    //}

    bool CanSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - headPosition.position;
        fAngleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);

        Debug.DrawRay(headPosition.position, playerDir);
        //Debug.Log(fAngleToPlayer);

        if(Physics.Raycast(headPosition.position, playerDir, out RaycastHit hit))
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

        if (!bIsShooting && fAngleToPlayer <= shootAngle)
        {
            StartCoroutine(Shoot());
        }
    }
    
    IEnumerator Shoot(){
        bIsShooting = true;//tell update that this is running
        anim.SetTrigger("Attack");//play the shooting animation
        yield return new WaitForSeconds(shootRate * 0.5f);
        CreateBullet();
        yield return new WaitForSeconds(shootRate * 0.5f);//cooldown
        bIsShooting = false;//tell update that we're ready to shoot again
    }

    public void CreateBullet()
    {
        Instantiate(bullet, shootPosition.position, transform.rotation);//create bullet
    }

    public void TakeDamage(int dmg)
    {
        iHP -= dmg;//health goes down
        StartCoroutine(ShowHealth());
        hpBar.value = iHP;
        StartCoroutine(FlashColor());//indicate damage taken
        navAgent.SetDestination(gameManager.instance.player.transform.position);
        StartCoroutine(BeenShot());

        if(iHP <= 0) //if it dies, get rid of it
        {
            anim.SetTrigger("Died");//play the death animation
            if (Random.Range(0, 100) <= DropRate && drop != null)
            {
                Instantiate(drop, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), transform.rotation);
            }
            gameManager.instance.playerScript.ChargeUt(chargeValue);
            --levelManager.enemiesRemaining;
            Destroy(gameObject);
        }
        else 
        {
            anim.SetTrigger("GetHit");//play the hurt animation
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

    public void Knockback(Vector3 dir)
    {
        navAgent.velocity += dir;
    }
}
