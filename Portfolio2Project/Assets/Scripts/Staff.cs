using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour
{
    [SerializeField] GameObject basicBullet;
    [SerializeField] private bool addBulletSpread;
    [SerializeField] private Vector3 bulletSpread = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] private ParticleSystem shootingSystem;
    [SerializeField] private Transform shootPos;
    [SerializeField] private ParticleSystem impactParticles;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private float delay;
    [SerializeField] private LayerMask mask;

    private Animator anim;
    private float lastShootTime;

    // Start is called before the first frame update
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }

    public void Shoot()
    {
        if (Input.GetButton("Shoot"))
        {
        if (lastShootTime + delay < Time.time)
        {
            //anim.SetBool("IsShooting", true);
            Vector3 direction = GetDirection();
            if(Physics.Raycast(shootPos.position, direction, out RaycastHit hit, float.MaxValue, mask))
            {
                TrailRenderer trail = Instantiate(trailRenderer, shootPos.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail, hit));

                lastShootTime = Time.time;
            }
        }
            
        }
    } 

    private Vector3 GetDirection()
    {
        Vector3 dir = transform.forward;

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
        anim.SetBool("isShooting", false);
        trail.transform.position = hit.point;
        Instantiate(impactParticles, hit.point, Quaternion.LookRotation(hit.normal));

        Destroy(trail.gameObject, trail.time);
            
    }
}
