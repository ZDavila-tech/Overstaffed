using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour, IDamage, IPhysics
{
    LevelManager levelManager;
    GameObject player;

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
    [SerializeField] float turnRate;
    [SerializeField] float fFieldOfView;
    [SerializeField] float fChaseTime;
    [Range(0, 100)][SerializeField] int DropRate;
    [Range(0, 100)][SerializeField] List<int> itemRates;
    [SerializeField] int chargeValue;
    [SerializeField] int ExperienceYield;
    [SerializeField] float interuptionCoolDown;
    [SerializeField] float moveSpeed;
    [SerializeField] bool brokenAnimations;
    [SerializeField] Transform deathParticle;
    [SerializeField] Vector3 knockbackResistance;

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

    bool isShooting;
    bool playerInRange;
    public bool isSlowed;
    public bool spawnedBySpawner;
    bool interrupted;
    bool isStopped;

    Vector3 playerDirection;
    float angleToPlayer;

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

        if(gameManager.instance != null)
        {
            player = gameManager.instance.playerCharacter;
        }
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
        if(player != null)
        {
            if (hpDisplay.activeSelf)
            {
                hpDisplay.transform.LookAt(player.transform.position);
            }

            if (navAgent.isActiveAndEnabled)
            {
                navAgent.SetDestination(gameManager.instance.playerCharacter.transform.position);

                if (CanSeePlayer() && playerInRange)
                {
                    AttackPlayer();
                }
            }
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
            isShooting = false;
            anim.speed = 1;
            isStopped = false;
        }
        else
        {
            navAgent.isStopped = true;
            isShooting = true;
            anim.speed = 0;
            isStopped = true;
        }
    }

    bool CanSeePlayer()
    {
        playerDirection = player.transform.position - headPosition.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDirection.x, 0, playerDirection.z), transform.forward);

        Debug.DrawRay(headPosition.position, playerDirection);
        //Debug.Log(fAngleToPlayer);

        if (Physics.Raycast(headPosition.position, playerDirection, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= fFieldOfView)
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
            playerDirection = player.transform.position - headPosition.position;
            Quaternion desiredRotation = Quaternion.LookRotation(new Vector3(playerDirection.x, 0, playerDirection.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * turnRate);
        }
    }

    void AttackPlayer()
    {
        if (navAgent.remainingDistance < navAgent.stoppingDistance)
        {
            FacePlayer();
        }

        if (!isShooting && angleToPlayer <= shootAngle)
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
        isShooting = true;//tell update that this is running
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
        isShooting = false;//tell update that we're ready to shoot again
    }

    IEnumerator Explode()
    {
        isShooting = true;
        if(angleToPlayer > shootAngle)
        {
            isShooting = false;
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
            var par = Instantiate(deathParticle, transform.position, transform.rotation);
            Destroy(gameObject);
            Destroy(par.gameObject, 1);
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
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void Knockback(Vector3 dir)
    {
        navAgent.velocity += (dir - knockbackResistance);
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
        StartCoroutine(Burning(duration, timeBetween, 1));
    }
}
