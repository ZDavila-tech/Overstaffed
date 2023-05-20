using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewStaff : MonoBehaviour
{
    [Header("----- Shooting Stuff -----")]
    [SerializeField] private bool addBulletSpread;
    [SerializeField] private Vector3 bulletSpread = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] private ParticleSystem shootingSystem;
    [SerializeField] private Transform shootPos;
    [SerializeField] private ParticleSystem impactParticles;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private float delay;
    [SerializeField] private LayerMask mask;
    [Header("----- Melee Stuff -----")]
    [SerializeField] private float meleeCooldown;
    [SerializeField] Collider hitbox;
    [SerializeField] private Renderer sWeapon;

    public enum Element
    {
        Fire,
        Water,
        Earth
    }
    public Element element;

    //[SerializeField] Animator anim;
    private float lastShootTime;
    //private float timer;
    public bool isShooting;
    public bool canMelee;
    public GameObject weapon;
    public bool isAttacking;

    private void Awake()
    {
        hitbox.enabled = false;
        sWeapon.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking)
        {
            sWeapon.enabled = true;
        }
        else
        {
            sWeapon.enabled = false;
        }
        //Shoot();
        Melee();
    }

    public void Shoot()
    {

        if (Input.GetButton("Shoot"))
        {
            isShooting = true;
            canMelee = false;
            if (lastShootTime + delay < Time.time)
            {
                Vector3 direction = GetDirection();
                if (Physics.Raycast(shootPos.position, direction, out RaycastHit hit, float.MaxValue, mask))
                {
                    TrailRenderer trail = Instantiate(trailRenderer, shootPos.position, Quaternion.identity);
                    StartCoroutine(SpawnTrail(trail, hit));
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
            trail.transform.position = Vector3.Lerp(startPos, hit.point, time);
            time += Time.deltaTime / trail.time;
            yield return null;
        }

        trail.transform.position = hit.point;

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
            isAttacking = true;
            hitbox.enabled = true;
            canMelee = false;
            Animator anim = weapon.GetComponent<Animator>();
            switch (element)
            {
                case Element.Fire:
                    anim.SetTrigger("SwordMelee");
                    break;
                case Element.Water:
                    anim.SetTrigger("SpearMelee");
                    break;
                case Element.Earth:
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
        yield return new WaitForSeconds(1);
        hitbox.enabled = false;
        canMelee = true;
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

    public int GetElement()
    {
        return (int)element;
    }
}