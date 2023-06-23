using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPushBackObject : MonoBehaviour
{
    [Header("----- Set by Detection Zone (Ignore) -----")]
    public GameObject player;

    private Vector3 targetPosition;

    void Start()
    {
        gameObject.transform.LookAt(player.transform.position);
        targetPosition = player.transform.position;
        StartCoroutine(LerpPosition(targetPosition, 1));
        Destroy(gameObject, 1.1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IPhysics>() != null)
        {
            IPhysics physicsable = other.GetComponent<IPhysics>();

            Vector3 dir = other.transform.position - transform.position;
            physicsable.Knockback(dir * 15);
        }
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }
}
