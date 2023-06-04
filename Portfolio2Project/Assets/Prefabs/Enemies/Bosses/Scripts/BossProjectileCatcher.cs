using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectileCatcher : MonoBehaviour
{
    public GameObject expectedProjectile;

    private void OnTriggerEnter(Collider other)
    {
        if (expectedProjectile != null)
        {
            if (other.gameObject == expectedProjectile)
            {
                //Do an explosion or something
                Destroy(expectedProjectile);
            }
        }
    }
}
