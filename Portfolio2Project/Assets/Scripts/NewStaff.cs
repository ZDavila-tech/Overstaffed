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
    [SerializeField] private Transform shootPos;
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

    
    private bool canSpecial;
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

    [Header("----- Audio Stuff -----")]
    [SerializeField] List<AudioClip> audios = new List<AudioClip>();

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
        

        //switch (playerElement)
        //{
        //    case Element.Fire:
        //        weaponModels[0].GetComponent<MeshRenderer>().material.SetTexture("_MainTex", fireStaff);
        //        break;
        //    case Element.Water:
        //        weaponModels[0].GetComponent<MeshRenderer>().material.SetTexture("_MainTex", waterStaff);
        //        break;
        //    case Element.Earth:
        //        weaponModels[0].GetComponent<MeshRenderer>().material.SetTexture("_MainTex", earthStaff);
        //        break;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (playerElement != player.playerElement)
        {
            SetElement();
        }
        Melee();
        SpecialAttack();
        //if(waterEffect != null)
        //{
        //    waterEffect.transform.position = player.transform.position;
        //}
        
    }

    public void Shoot()
    {
        Debug.Log("Weapon Shoot Called");
        if (Input.GetButton("Shoot"))
        {

            isShooting = true;
            canMelee = false;
            if (lastShootTime + delay < Time.time)
            {
                Vector3 direction = GetDirection();
                if (Physics.Raycast(shootPos.position, direction, out RaycastHit hit, float.MaxValue, mask))
                {
                    GameObject trail = Instantiate(trailRenderer[(int)playerElement].gameObject, shootPos.position, Quaternion.identity);
                    StartCoroutine(SpawnTrail(trail.GetComponent<TrailRenderer>(), hit));
                    lastShootTime = Time.time;
                }
            }
        }
        StartCoroutine(ResetShooting());
    }

    private Vector3 GetDirection()
    {
        Vector3 dir = transform.parent.forward;

        if (addBulletSpread)
        {
            dir += new Vector3(
                Random.Range(bulletSpread.x, bulletSpread.x),
                Random.Range(bulletSpread.y, bulletSpread.y),
                Random.Range(bulletSpread.z, bulletSpread.z)
                );
        }
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
        if (canMelee && Input.GetMouseButtonDown(1))
        {
            canMelee = false;
            isAttacking = true;

            Animator anim = weapon.GetComponent<Animator>();
            //weaponModels[0].SetActive(false);
            switch (playerElement)
            {
                case Element.Fire:
                    Debug.Log("Melee Fire");
                    swordHitbox.enabled = true;
                    weaponModels[1].SetActive(true);
                    gameManager.instance.playerScript.PlayExternalAudio(audios[3]);
                    anim.SetTrigger("SwordMelee");
                    break;
                case Element.Water:
                    weaponModels[2].SetActive(true);
                    spearHitbox.enabled = true;
                    gameManager.instance.playerScript.PlayExternalAudio(audios[4]);
                    anim.SetTrigger("SpearMelee");
                    break;
                case Element.Earth:
                    weaponModels[3].SetActive(true);
                    hammerHitbox.enabled = true;
                    gameManager.instance.playerScript.PlayExternalAudio(audios[5]);
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
        isAttacking = false;
        hammerHitbox.enabled = false;
        spearHitbox.enabled = false;
        swordHitbox.enabled = false;
    }

    IEnumerator ResetShooting()
    {
        yield return new WaitForSeconds(0.5f);
        isShooting = false;
        canMelee = true;
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
    }


    public AudioClip GetShootAudio()
    {
        return audios[(int)playerElement];
    }

    

    IEnumerator WaterAOE()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies)
        {
            if (wSpecialRange >= Vector3.Distance(transform.position, enemy.transform.position))
            {
                Instantiate(waterEffect, player.transform.position, Quaternion.identity);
                enemy.GetComponent<NavMeshAgent>().speed /= 2;
                if (enemy.GetComponent<IDamage>() != null)
                {
                    int timesDamaged = 0;
                    while (true)
                    {
                        //waterEffect.enableEmission = false;
                        enemy.GetComponent<EnemyAI>().TakeDamage(1);
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
        foreach (GameObject enemy in enemies)
        {
            if (eSpecialRange >= Vector3.Distance(transform.position, enemy.transform.position))
            {

                enemy.GetComponent<EnemyAI>().TakeDamage(eSpecialDamage);
            }
        }
    }

    public void SpecialAttack()
    {
        if (Input.GetButtonUp("Special"))   //When the button is held and then released
        {
            if (!canSpecial)
            {
                return;
            }
            //if(!player.canUt())
            //{ return; }

            switch (playerElement)
                {
                    case Element.Fire:

                    player.ChargeUt(-100);
                    RaycastHit hit;
                    Vector3 direction = GetDirection();
                    if (Physics.Raycast(shootPos.position, direction, out hit, float.MaxValue, mask))
                    {
                        if (hit.transform.tag == "Player")
                        {
                            Physics.IgnoreCollision(hit.collider, player.GetComponent<Collider>());
                        }

                        Instantiate(explosionEffect, hit.point, Quaternion.LookRotation(hit.normal));
                        Instantiate(explosion, hit.point, Quaternion.LookRotation(hit.normal));
                    }

                    break;
                    case Element.Water:

                    StartCoroutine(WaterAOE());

                        break;
                    case Element.Earth:

                        EarthAOE();

                    ResetShooting();

                        break;
                }
            if (player.canUt())
            {
            }
            }

        }
}