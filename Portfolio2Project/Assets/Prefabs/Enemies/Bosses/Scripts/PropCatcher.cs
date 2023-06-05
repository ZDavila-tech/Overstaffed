using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropCatcher : MonoBehaviour
{
    public GameObject expectedProjectile;

    private void OnTriggerEnter(Collider other)
    {
        if (expectedProjectile != null)
        {
            if (other.gameObject == expectedProjectile)
            {
                //make a crashing sound
                expectedProjectile.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
    }
}
