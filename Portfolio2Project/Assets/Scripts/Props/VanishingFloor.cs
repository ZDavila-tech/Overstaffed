using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishingFloor : MonoBehaviour
{
    [SerializeField] float vanishDelay;
    [SerializeField] float appearDelay;
    [SerializeField] Transform particle;

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
        this.gameObject.SetActive(false);
    }

    IEnumerator appear()
    {
        yield return new WaitForSeconds(appearDelay);
        this.gameObject.SetActive(true);
    }
}
