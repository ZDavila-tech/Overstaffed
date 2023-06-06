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
    [SerializeField] List<GameObject> drop;
    [SerializeField] Animator anim;
    [SerializeField] MeshRenderer freezeEffect;

    [Header("----- Enemy Stats -----")]
    [SerializeField] int iHP;
    [SerializeField] Slider hpBar;
    [SerializeField] GameObject hpDisplay;
    [SerializeField] float fTurnRate;
    [SerializeField] float fFieldOfView;
    [SerializeField] float fChaseTime;
    [Range(0, 100)][SerializeField] int DropRate;
    [Range(0, 100)][SerializeField] List<int> itemRates;
    [SerializeField] int chargeValue;
    [SerializeField] int ExperienceYield;
    [SerializeField] float interuptionCoolDown;
    [SerializeField] float moveSpeed;
    [SerializeField] bool brokenAnimations;

    [Header("----- Weapon Stats -----")]
    [SerializeField] GameObject bullet;
    [Range(1, 100)][SerializeField] int shootDistance;
    [Range(0, 2.5f)][SerializeField] float shootRate;
    [SerializeField] float shootAngle;

    [Header("----- Specific Enemy Stuff -----")]
    [SerializeField] Category category;
    enum Category
    {
        Shooting,
        Exploding
    }
    //for the exploding enemy
    [SerializeField] float pointOfNoReturn;
    [SerializeField] float explodeTime;
    [SerializeField] GameObject explosion;

    bool bIsShooting;
    bool bPlayerInRange;
    bool bBeenShot;
    public bool isSlowed;
    public bool spawnedBySpawner;
    bool interrupted;
    bool isStopped;

    Vector3 playerDir;
    float fAngleToPlayer;

    //IDamage damageInterface;
    private Color cOrigColor;

    void Start()
    {
        isSlowed = false;
        if (LevelManager.instance != null)
        {
            levelManager = LevelManager.instance;
        }

        cOrigColor = rModel.material.color;

        hpBar.maxValue = iHP;
        hpBar.value = hpBar.maxValue;
        if (anim != null)
        {
            anim.SetFloat("Shoot Rate", 1 / shootRate);
        }

        if (spawnedBySpawner == false)
        {
            ++levelManager.enemiesRemaining;
        }

        interrupted = false;
    }


    void Update()
    {
        //StartCoroutine(SlowEnemy());
        //if (anim != null)
        //{
        //    float speed = 0;
        //    speed = Mathf.Lerp(speed, navAgent.velocity.normalized.magnitude, Time.deltaTime * animTransSpeed);
        //    anim.SetFloat("Speed", speed);
        //}

        if (hpDisplay.activeSelf)
        {
            hpDisplay.transform.LookAt(gameManager.instance.playerCharacter.transform.position);
        }
        if ((bPlayerInRange || bBeenShot) && CanSeePlayer())
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
    public void toggleMovement(bool toggle) //enables/disables movement, shooting and animation
    {
        if (toggle)
        {
            navAgent.isStopped = false;
            bIsShooting = false;
            anim.speed = 1;
            isStopped = false;
        }
        else
        {
            navAgent.isStopped = true;
            bIsShooting = true;
            anim.speed = 0;
            isStopped = true;
        }
    }

    bool CanSeePlayer()
    {
        playerDir = gameManager.instance.playerCharacter.transform.position - headPosition.position;
        fAngleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);

        Debug.DrawRay(headPosition.position, playerDir);
        //Debug.Log(fAngleToPlayer);

        if (Physics.Raycast(headPosition.position, playerDir, out RaycastHit hit))
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
        if (!isStopped)
        {
            Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * fTurnRate);
        }
    }

    void AttackPlayer()
    {
        if (navAgent.isActiveAndEnabled)
        {
            navAgent.SetDestination(gameManager.instance.playerCharacter.transform.position);
        }

        if (navAgent.remainingDistance < navAgent.stoppingDistance)
        {
            //Debug.Log("YARGH");
            FacePlayer();

        }


        if (!bIsShooting && fAngleToPlayer <= shootAngle)
        {
            switch (category)
            {
                case Category.Shooting:
                    StartCoroutine(Shoot());
                    break;
                case Category.Exploding:
                    StartCoroutine(Explode());
                    break;
            }
        }
    }

    IEnumerator Shoot()
    {
        bIsShooting = true;//tell update that this is running
        if (brokenAnimations == true)
        {
            yield return new WaitForSeconds(shootRate * 0.5f);
            CreateBullet();
            yield return new WaitForSeconds(shootRate * 0.5f);
        }
        else
        {
            anim.SetTrigger("Attack");//play the shooting animation
            yield return new WaitForSeconds(shootRate);
        }
        bIsShooting = false;//tell update that we're ready to shoot again
    }

    IEnumerator Explode()
    {
        bIsShooting = true;
        if(fAngleToPlayer > shootAngle)
        {
            bIsShooting = false;
            StopCoroutine(Explode());
        }
        yield return new WaitForSeconds(pointOfNoReturn);
        yield return new WaitForSeconds(explodeTime);
        Instantiate(explosion, transform.position, explosion.transform.rotation);
        Destroy(gameObject);
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
        if (navAgent.isActiveAndEnabled)
        {
            navAgent.SetDestination(gameManager.instance.playerCharacter.transform.position);
        }
        StartCoroutine(BeenShot());

        if (iHP <= 0) //if it dies, get rid of it
        {
            anim.SetTrigger("Died");//play the death animation
            if (Random.Range(0, 100) <= DropRate && drop.Count > 0)
            {
                //Instantiate(drop, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
                Drop();
            }
            gameManager.instance.playerStats.GainExp(ExperienceYield);
            gameManager.instance.playerController.ChargeUt(chargeValue);
            --levelManager.enemiesRemaining;
            Destroy(gameObject);
        }
        else
        {
            if (interrupted == false)
            {
                StartCoroutine(GetInterrupted());
            }
        }
    }

    void Drop()
    {
        int total = 0;
        foreach (int itemRate in itemRates)
        {
            total += itemRate;
        }
        int currentRate = 0;
        for (int i = 0; i < itemRates.Count; i++)
        {
            currentRate += itemRates[i];
            if (currentRate >= Random.Range(0, total))
            {
                Instantiate(drop[i], new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
                return;
            }
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
        if (other.CompareTag("Player"))
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

    IEnumerator GetInterrupted()
    {
        interrupted = true;
        anim.SetTrigger("GetHit");//play the hurt animation

        yield return new WaitForSeconds(interuptionCoolDown);

        interrupted = false;
    }


    IEnumerator Freezing(float duration)
    {
        toggleMovement(false);
        freezeEffect.enabled = true;
        yield return new WaitForSeconds(duration);
        toggleMovement(true);
        freezeEffect.enabled = false;
    }

    public void Freeze(float duration)
    {
        StartCoroutine(Freezing(duration));
    }

    IEnumerator Burning(float duration, float timeBetween, int damage)
    {
        bool burning = true;
        while (burning)
        {
            yield return new WaitForSeconds(timeBetween);
            TakeDamage(damage);
        }
        yield return new WaitForSeconds(duration);
        burning = false;
    }

    public void Burn (float duration, float timeBetween)
    {

    }
}