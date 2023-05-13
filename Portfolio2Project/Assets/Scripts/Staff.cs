using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Staff : MonoBehaviour
{
    [SerializeField] private bool addBulletSpread;
    [SerializeField] private Vector3 bulletSpread = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] private ParticleSystem shootingSystem;
    [SerializeField] private Transform shootPos;
    [SerializeField] private ParticleSystem impactParticles;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private float delay;
    [SerializeField] private LayerMask mask;
    [SerializeField] private float meleeCooldown;


    //[SerializeField] Animator anim;
    private float lastShootTime;
    private float timer;
    bool isShooting;
    public bool canMelee;
    public GameObject weapon;

    private void Awake()
    {
        //anim = gameObject.GetComponent<Animator>();
        //anim.speed = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        //timer += Time.deltaTime;
        Shoot();
        Melee();
        

        
    }

    public void Shoot()
    {
        
        if (Input.GetButton("Shoot"))
        {
            isShooting = true;
            //anim.speed = 1.5f;
            if (lastShootTime + delay < Time.time)
            {
                //anim.SetBool("IsShooting", true);
                Vector3 direction = GetDirection();
                if(Physics.Raycast(shootPos.position, direction, out RaycastHit hit, float.MaxValue, mask))
                {
                    TrailRenderer trail = Instantiate(trailRenderer, shootPos.position, Quaternion.identity); //Quaternion.identity

                        StartCoroutine(SpawnTrail(trail, hit));

                    lastShootTime = Time.time;
                    
                    //StartCoroutine(def.DestroyTrail(trailRenderer));
                    //StartCoroutine(def.DestroyParticle(impactParticles));
                }
            }
            
        }
        //yield return new WaitForSeconds(1);
        //isShooting = false;
        //anim.speed = 0.5f;
    } 

    private Vector3 GetDirection()
    {
        Vector3 dir = transform.parent.forward;
        //Vector3 dir = new Vector3(transform.parent.rotation.x, transform.parent.rotation.y, transform.parent.rotation.z);

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
        //Vector3 startPos = trail.transform.position;
        Vector3 startPos = shootPos.transform.position;

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPos, hit.point, time);
            time += Time.deltaTime / trail.time;
            yield return null;
        }
        //anim.SetBool("isShooting", false);
        trail.transform.position = hit.point;
        if(impactParticles != null)
        {
        }
            Instantiate(impactParticles, hit.point, Quaternion.LookRotation(hit.normal)); //hit.point

        if (trail.gameObject != null)
        {
            yield return new WaitForSeconds(1);
            Destroy(trail.gameObject);
        }

    }

    public void Melee()
    {
        if (!canMelee)
        {
            return;
        }


        if (Input.GetMouseButtonDown(1))
        {
            canMelee = false;
            Animator anim = weapon.GetComponent<Animator>();
            anim.SetTrigger("Melee");
            StartCoroutine(ResetMeleeCooldown());
        }
    }
    IEnumerator ResetMeleeCooldown()
    {
        yield return new WaitForSeconds(meleeCooldown);
        canMelee = true;
    }
}
