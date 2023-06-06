using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropCatcher : MonoBehaviour
{
    public GameObject expectedProjectile;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == expectedProjectile)
        {
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
