using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    EnemySpawner associatedSpawner;

    private void Start()
    {
        associatedSpawner = this.gameObject.GetComponentInParent<EnemySpawner>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other == CompareTag("Player"))
        {
            associatedSpawner.playerDetected = true;
        }
    }
}
