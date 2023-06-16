using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour, IDamage, IPhysics
{
    enum shotType
    {
        single,
        spread
    }
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
    [SerializeField] GameObject poisonEffect;
    [SerializeField] GameObject burnEffect;

    [Header("----- Enemy Stats -----")]
    [SerializeField] int iHP;
    [SerializeField] Slider hpBar;
    [SerializeField] GameObject hpDisplay;
    [SerializeField] TextMeshPro damageNumbers;
    [SerializeField] float turnRate;
    [SerializeField] float fFieldOfView;
    [Range(0, 100)][SerializeField] int DropRate;
    [Range(0, 100)][SerializeField] List<int> itemRates;
    [SerializeField] int chargeValue;
    [SerializeField] int ExperienceYield;
    [SerializeField] float interuptionCoolDown;
    [SerializeField] float moveSpeed;
    [SerializeField] bool brokenAnimations;
    [SerializeField] Transform deathParticle;
    [SerializeField] Vector3 knockbackResistance;
    [SerializeField] int damageDealt;
    [SerializeField] shotType ShotType = shotType.single;
    [SerializeField, Range(0, 1)] float bulletSprayVar;
    [SerializeField, Range(0, 10)] float spreadShotSize;
    [SerializeField] int spreadShotCount;

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
    public bool isSlowed;
    public bool spawnedBySpawner;
    bool interrupted;

    Vector3 playerDirection;
    float angleToPlayer;
    float playerDistance;

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
        if (player != null)
        {

            if (hpDisplay.activeSelf)
            {
                hpDisplay.transform.LookAt(player.transform.position);
            }
            if (navAgent.enabled)
            {
                navAgent.SetDestination(player.transform.position);

                if (CanSeePlayer() && playerDistance <= shootDistance)
                {
                    AttackPlayer();
                }
                
                if(navAgent.remainingDistance < navAgent.stoppingDistance){
                    FacePlayer();
                }
            }
        }
        else
        {
            player = gameManager.instance.playerCharacter;
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
            anim.speed = 1;
        }
        else
        {
            anim.speed = 0;
        }
        navAgent.isStopped = toggle;
        isShooting = toggle;
    }

    bool CanSeePlayer()
    {
        playerDirection = (player.transform.position - headPosition.position);
        angleToPlayer = Vector3.Angle(new Vector3(playerDirection.x, 0, playerDirection.z), transform.forward);

        Debug.DrawRay(headPosition.position, playerDirection);

        if (Physics.Raycast(headPosition.position, playerDirection, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer < fFieldOfView / 2)
            {
                playerDistance = hit.distance;
                return true;
            }
            
        }

        return false;
    }

    void FacePlayer()
    {
        if (!navAgent.isStopped)
        {
            Vector3 direction = (player.transform.position - gameObject.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, lookRotation, Time.deltaTime * turnRate);
        }
    }

    void AttackPlayer()
    {

        if (!isShooting)
        {
            switch (category)
            {
                case Category.Shooting:
                    AudioManager.instance.EnemyShoot();
                    StartCoroutine(Shoot());
                    break;
                case Category.Exploding:
                    AudioManager.instance.EnemyExpShoot();
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
        if (angleToPlayer > shootAngle)
        {
            isShooting = false;
            StopCoroutine(Explode());
        }
        yield return new WaitForSeconds(pointOfNoReturn);
        yield return new WaitForSeconds(explodeTime);
        Instantiate(explosion, transform.position, explosion.transform.rotation);
        --levelManager.enemiesRemaining;
        Destroy(gameObject);
    }

    public void CreateBullet()
    {
        if (ShotType == shotType.single)
        {

            GameObject proj = Instantiate(bullet, shootPosition.position, Quaternion.identity) ;//create bullet
            Projectile projSet;
            if (proj.TryGetComponent<Projectile>(out projSet))
            {
                projSet.SetDamage(damageDealt);
                projSet.SetVariance(bulletSprayVar);
            }
        }
        else if (ShotType == shotType.spread)
        {
            for (int i = 0; i < spreadShotCount; i++)
            {
                GameObject proj = Instantiate(bullet, shootPosition.position, Quaternion.identity);//create bullet
                Projectile projSet;
                if (proj.TryGetComponent<Projectile>(out projSet))
                {
                    projSet.SetDamage(damageDealt);
                    projSet.SetVariance(spreadShotSize);
                }
            }
        }

    }

    public void TakeDamage(int dmg)
    {
        iHP -= dmg;//health goes down
        StartCoroutine(ShowHealth());
        StartCoroutine(ShowDamage(dmg));
        hpBar.value = iHP;
        StartCoroutine(FlashColor());//indicate damage taken
        if (navAgent.isActiveAndEnabled)
        {
            navAgent.SetDestination(gameManager.instance.playerCharacter.transform.position);
        }

        if (iHP <= 0) //if it dies, get rid of it
        {

            AudioManager.instance.EnemyDeath();
            anim.SetTrigger("Died");//play the death animation
            if (Random.Range(0, 100) <= DropRate && drop.Count > 0)
            {
                //Instantiate(drop, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
                Drop();
            }
            gameManager.instance.playerStats.GainExp(ExperienceYield);
            gameManager.instance.playerController.ChargeUt(chargeValue);
            --levelManager.enemiesRemaining;
            ++levelManager.totalEnemiesDefeated;
            var par = Instantiate(deathParticle, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.rotation);
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
                Instantiate(drop[i], new Vector3(transform.position.x, transform.position.y + 0.9f, transform.position.z), transform.rotation);
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
    IEnumerator ShowDamage(int dmg)
    {
        damageNumbers.enabled = true;
        damageNumbers.text = dmg.ToString();
        StartCoroutine(DmgNumAnim());
        yield return new WaitForSeconds(0.5f);
        damageNumbers.enabled = false;
    }

    IEnumerator DmgNumAnim()
    {
        Vector3 originalpos = damageNumbers.transform.position;
        bool moving = true;
        while (moving) 
        {
            yield return new WaitForSeconds(0.05f);
            damageNumbers.transform.position += Vector3.down;
            damageNumbers.transform.position += Vector3.right;
        }
        yield return new WaitForSeconds(0.5f);
        damageNumbers.transform.position = originalpos;
        moving = false;
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
        burnEffect.SetActive(true);
        damageDealt = (int)(damageDealt * 0.5);
        while (burning)
        {
            yield return new WaitForSeconds(timeBetween);
            TakeDamage(damage);
        }
        yield return new WaitForSeconds(duration);
        burning = false;
        burnEffect.SetActive(false);
        damageDealt *= 2;
    }

    IEnumerator Venom(float duration, float timeBetween, int damage)
    {
        bool poisoned = true;
        poisonEffect.SetActive(true);
        while (poisoned)
        {
            yield return new WaitForSeconds(timeBetween);
            TakeDamage(damage);
        }
        yield return new WaitForSeconds(duration);
        poisoned = false;
        poisonEffect.SetActive(false);
    }

    public void Burn(float duration, float timeBetween)
    {
        StartCoroutine(Burning(duration, timeBetween, 1));
    }

    public void Poison(float duration, float timeBetweeen)
    {
        StartCoroutine(Venom(duration, timeBetweeen, 1));
    }
}
