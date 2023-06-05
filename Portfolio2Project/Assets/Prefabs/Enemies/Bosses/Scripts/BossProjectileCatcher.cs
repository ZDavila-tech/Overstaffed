using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectileCatcher : MonoBehaviour
{
    public List<GameObject> expectedProjectile;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 14)
        {
            Destroy(other.gameObject);
        }
    }
}
