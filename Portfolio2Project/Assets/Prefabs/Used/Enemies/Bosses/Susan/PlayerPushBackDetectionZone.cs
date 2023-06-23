using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPushBackDetectionZone : MonoBehaviour
{
    [SerializeField] GameObject Susan;
    [SerializeField] GameObject pushBackProjectile;
    [SerializeField] GameObject spawnPointForProjectile;

    private bool isPushing;

    private void Awake()
    {
        isPushing = false;
    }

    private void Update()
    {
        if (Susan.GetComponent<SusanFromHR>().healthBarThree <= 0)
        {
            Destroy(gameObject);
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player") && !isPushing)
    //    {
    //        StartCoroutine(PushBackPlayer(other));
    //    }
    //}

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isPushing)
        {
            StartCoroutine(PushBackPlayer(other));
        }
    }

    IEnumerator PushBackPlayer(Collider playerCollider)
    {
        isPushing = true;

        GameObject pushBackObject = Instantiate(pushBackProjectile, spawnPointForProjectile.transform);
        pushBackObject.GetComponent<PlayerPushBackObject>().player = playerCollider.gameObject;

        yield return new WaitForSeconds(1.2f);

        isPushing = false;
    }
}
