using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class NewStaff : MonoBehaviour
{
    [Header("----- Shooting Stuff -----")]
    [SerializeField] private bool addBulletSpread;
    [SerializeField] private Vector3 bulletSpread = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] private ParticleSystem shootingSystem;
    public Transform shootPos;
    [SerializeField] private ParticleSystem impactParticles;
    [SerializeField] private List<TrailRenderer> trailRenderer;
    [SerializeField] private float delay;
    [SerializeField] private LayerMask mask;

    [Header("----- Melee Stuff -----")]
    [SerializeField] private float meleeCooldown;
    [SerializeField] Collider swordHitbox;
    [SerializeField] Collider spearHitbox;
    [SerializeField] Collider hammerHitbox;
    [SerializeField] GameObject[] weaponModels;
    [SerializeField] GameObject[] weaponParticles;
    //[SerializeField] Texture fireStaff;
    //[SerializeField] Texture waterStaff;
    //[SerializeField] Texture earthStaff;

    [Header("----- Fire Special Attack Stuff -----")]
    [SerializeField] public GameObject explosion;
    [SerializeField] public ParticleSystem explosionEffect;

    [Header("----- Water Special Attack Stuff -----")]
    [SerializeField] float wSpecialRange;
    [SerializeField] public float slowDuration;
    [SerializeField] GameObject waterEffect;
    GameObject[] enemies;

    [Header("----- Earth Special Attack Stuff -----")]
    [SerializeField] float eSpecialRange;
    [SerializeField] public int eSpecialDamage;
    [SerializeField] GameObject earthEffect;
    [SerializeField] GameObject eRangeIndicator;
    private GameObject indicatorEarth;

[Header("----- Other Stuff -----")]
    public bool canSpecial;
    [SerializeField] GameObject cWeapon;
    public List<Texture> materials;

    public enum Element
    {
        Fire,
        Water,
        Earth
    }
    public Element playerElement;

    PlayerController player;

    private float lastShootTime;
    public bool isShooting;
    public bool canMelee;
    public GameObject weapon;
    public bool isAttacking;


    private void Awake()
    {
        hammerHitbox.enabled = false;
        spearHitbox.enabled = false;
        swordHitbox.enabled = false;
        canSpecial = true;
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        SetWeaponModel();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerElement != player.playerElement)
        {
            SetElement();
            SetWeaponModel();
        }

        if (gameManager.instance.isPaused)
        {
            canMelee = false;
            canSpecial = false;
        }

        Melee();
        SpecialAttack();
        StartCoroutine(ResetShooting());
    }

    public void Shoot()
    {
        Animator anim = weapon.GetComponent<Animator>();
        //Debug.Log("Weapon Shoot Called");
        if (Input.GetButton("Shoot") && !isAttacking)
        {
            
            isShooting = true;
            canMelee = false;
            canSpecial = false;
            if (lastShootTime + delay < Time.time)
            {               
                if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out RaycastHit hit, float.MaxValue, mask))
                {
                    anim.SetTrigger("Shooting");
                    GameObject trail = Instantiate(trailRenderer[(int)playerElement].gameObject, shootPos.position, Quaternion.identity);
                    StartCoroutine(SpawnTrail(trail.GetComponent<TrailRenderer>(), hit));
                    lastShootTime = Time.time;
                }                
            }
        }
            //StartCoroutine(ResetShooting());
            StartCoroutine(ResetShootingAnimation());

    }

    private Vector3 GetSpread()
    {
        Vector3 dir = new Vector3(Random.Range(-bulletSpread.x, bulletSpread.x), Random.Range(-bulletSpread.y, bulletSpread.y), Random.Range(-bulletSpread.z, bulletSpread.z));
        return dir;
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPos = shootPos.transform.position;

        while (time < 1)
        {
            if (trail.gameObject != null)
            {
                trail.transform.position = Vector3.Lerp(startPos, hit.point, time);
                time += Time.deltaTime / trail.time;
            }
            yield return null;
        }

        if (trail.gameObject != null)
        {
            trail.transform.position = hit.point;
        }

        Instantiate(impactParticles, hit.point, Quaternion.LookRotation(hit.normal)); //hit.point

        if (trail.gameObject != null)
        {
            Destroy(trail.gameObject);
            yield return new WaitForSeconds(1);
        }
    }

    public void Melee()
    {
        if (canMelee && Input.GetMouseButtonDown(1) && !isShooting)
        {
            canMelee = false;
            canSpecial = false;
            isAttacking = true;

            Animator anim = weapon.GetComponent<Animator>();
            //weaponModels[0].SetActive(false);
            switch (playerElement)
            {
                case Element.Fire:
                    //Debug.Log("Melee Fire");
                    swordHitbox.enabled = true;
                    weaponModels[1].SetActive(true);
                    weaponParticles[0].SetActive(true);
                    AudioManager.instance.MeleeSound();
                    anim.SetTrigger("SwordMelee");
                    break;
                case Element.Water:
                    weaponModels[2].SetActive(true);
                    weaponParticles[1].SetActive(true);
                    spearHitbox.enabled = true;
                    AudioManager.instance.MeleeSound();
                    anim.SetTrigger("SpearMelee");
                    break;
                case Element.Earth:
                    weaponModels[3].SetActive(true);
                    weaponParticles[2].SetActive(true);
                    hammerHitbox.enabled = true;
                    AudioManager.instance.MeleeSound();
                    anim.SetTrigger("HammerMelee");
                    break;
            }

            StartCoroutine(ResetMeleeCooldown());
        }
    }
    IEnumerator ResetMeleeCooldown()
    {
        yield return new WaitForSeconds(meleeCooldown);
        canMelee = true;
        canSpecial = true;
        isAttacking = false;
        hammerHitbox.enabled = false;
        spearHitbox.enabled = false;
        swordHitbox.enabled = false;
    }

    IEnumerator ResetShooting()
    {
        if (Input.GetButton("Shoot") == false)
        {
            isShooting = false;
            canMelee = true;
            canSpecial = true;
        }
            yield return new WaitForSeconds(0.5f);
        
        
    }

    IEnumerator ResetShootingAnimation()
    {
        yield return new WaitForSeconds(1f);
        
        if(isShooting == false)
        {
            Animator anim = weapon.GetComponent<Animator>();
            anim.SetTrigger("NotShooting");
        }

        
    }

    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    public void SetElement()
    {
        playerElement = player.playerElement;
    }

    public void ResetMeleeWeaponModel()
    {
        //weaponModels[0].SetActive(true);
        weaponModels[1].SetActive(false);
        weaponModels[2].SetActive(false);
        weaponModels[3].SetActive(false);
        StartCoroutine(resetParticles());
    }


    IEnumerator resetParticles()
    {
        yield return new WaitForSeconds(0.5f);
        weaponParticles[0].SetActive(false);
        weaponParticles[1].SetActive(false);
        weaponParticles[2].SetActive(false);
    }


    public AudioClip GetShootAudio()
    {
        return AudioManager.instance.staffClips[(int)playerElement];
    }

    private void FireSpecial()
    {
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out RaycastHit hit, float.MaxValue, mask))
        {
            if (hit.transform.CompareTag("Player"))
            {
                Physics.IgnoreCollision(hit.collider, player.GetComponent<Collider>());
            }

            Instantiate(explosionEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Instantiate(explosion, hit.point, Quaternion.LookRotation(hit.normal));
        }
    }

    IEnumerator WaterAOE()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies)
        {
            if(enemy == null)
            {
                Debug.Log("Returned");
                break;
            }
            if (wSpecialRange >= Vector3.Distance(transform.position, enemy.transform.position))
            {
                Instantiate(waterEffect, player.transform.position, Quaternion.identity);
                enemy.GetComponent<NavMeshAgent>().speed /= 2;
                if (enemy.GetComponent<IDamage>() != null)
                {
                    int timesDamaged = 0;
                    enemy.GetComponent<EnemyAI>().Freeze(slowDuration);
                    while (true)
                    {
                        //waterEffect.enableEmission = false;
                        //enemy.GetComponent<EnemyAI>().TakeDamage(1);
                        player.GetComponent<IDamage>().TakeDamage(-2);
                        timesDamaged++;
                        yield return new WaitForSeconds(1);
                        if (timesDamaged == slowDuration)
                        {
                            break;
                        }
                    }
                }
                //yield return new WaitForSeconds(slowDuration);
                enemy.GetComponent<NavMeshAgent>().speed *= 2;
            }
        }
    }

    void EarthAOE()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out RaycastHit hit, float.MaxValue, mask))
        //{
        //    Instantiate(earthEffect, hit.point, earthEffect.transform.rotation);
        //}
        foreach (GameObject enemy in enemies)
        {
            if (eSpecialRange >= Vector3.Distance(transform.position, enemy.transform.position))
            {
                enemy.GetComponent<EnemyAI>().TakeDamage(eSpecialDamage + Stats.Attack);
            }
        }
    }


    public void SpecialAttack()
    {
        if (!canSpecial || !player.CanUltimate())
        {
            return;
        }

        Animator anim = weapon.GetComponent<Animator>();
        if (Input.GetButtonDown("Special"))
        {
            canMelee = false;
            isAttacking = true;
            isShooting = true;
            player.isShooting = true;
            switch (playerElement)
            {
                case Element.Fire:
                    anim.SetTrigger("FSpecialHold");
                    weaponParticles[0].SetActive(true);
                    break;
                case Element.Water:
                    anim.SetTrigger("WSpecialHold");
                    weaponParticles[1].SetActive(true);
                    break;
                case Element.Earth:
                    anim.SetTrigger("ESpecialHold");
                    weaponParticles[2].SetActive(true);
                    indicatorEarth = Instantiate(eRangeIndicator, player.transform.position, eRangeIndicator.transform.rotation);
                    break;
            }
        }

        if (Input.GetButtonUp("Special"))   //When the button is held and then released
        {
            switch (playerElement)
            {
                case Element.Fire:

                    anim.SetTrigger("FSpecialRelease");
                    player.ChargeUt(-100);
                    FireSpecial();
                    break;

                case Element.Water:

                    anim.SetTrigger("WSpecialRelease");
                    player.ChargeUt(-100);
                    StartCoroutine(WaterAOE());
                    break;

                case Element.Earth:

                    anim.SetTrigger("ESpecialRelease");
                    player.ChargeUt(-100);
                    if(indicatorEarth != null)
                    {
                        Destroy(indicatorEarth);
                    }

                    EarthAOE();
                    ResetShooting();
                    break;
            }
            canMelee = true;
            isAttacking = false;
            isShooting = false;
            player.isShooting = false;
        }
        resetParticles();
    }

    public void SetWeaponModel()
    {
        switch (player.playerElement)
        {
            case Element.Fire:
                //weaponModels[0].GetComponent<MeshRenderer>().material.SetTexture("_MainTex", fireStaff);
                cWeapon.GetComponent<Renderer>().material.SetTexture("_MainTex", materials[0]); //= materials[0];
                break;
            case Element.Water:
                //weaponModels[0].GetComponent<MeshRenderer>().material.SetTexture("_MainTex", waterStaff);
                cWeapon.GetComponent<Renderer>().material.SetTexture("_MainTex", materials[1]);
                break;
            case Element.Earth:
                //weaponModels[0].GetComponent<MeshRenderer>().material.SetTexture("_MainTex", earthStaff);
                cWeapon.GetComponent<Renderer>().material.SetTexture("_MainTex", materials[2]);
                break;
        }
    }
}