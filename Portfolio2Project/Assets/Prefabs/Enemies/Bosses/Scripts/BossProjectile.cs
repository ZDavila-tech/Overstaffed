using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    [SerializeField] Rigidbody rigidBody;
    [SerializeField] float projectileSpeed;

    [Header("----- Set By Boss (Ignore) -----")]
    public Transform spawnPosition;
    public Transform targetToMoveTo;

    private void Start()
    {
        rigidBody.velocity = (targetToMoveTo.position - spawnPosition.position).normalized * projectileSpeed;
    }
}
