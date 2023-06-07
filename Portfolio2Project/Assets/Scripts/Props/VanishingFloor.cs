using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishingFloor : MonoBehaviour
{
    [SerializeField] float vanishDelay;
    [SerializeField] float appearDelay;
    [SerializeField] Transform particle;
    [SerializeField] GameObject platform;

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(vanish());
    }

    IEnumerator vanish()
    {
        yield return new WaitForSeconds(vanishDelay);
        ParticleSystem particleComp = Instantiate(particle,transform.position,transform.rotation).GetComponent<ParticleSystem>();
        var shape = particleComp.shape;
        shape.scale = transform.localScale;
        StartCoroutine(appear());
        platform.gameObject.SetActive(false);
    }

    IEnumerator appear()
    {

        yield return new WaitForSeconds(appearDelay);
        Debug.Log("Appearing");
        platform.gameObject.SetActive(true);
    }
}
