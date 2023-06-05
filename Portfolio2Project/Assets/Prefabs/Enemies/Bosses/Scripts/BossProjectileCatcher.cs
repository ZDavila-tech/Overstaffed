using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectileCatcher : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 14)
        {
            Destroy(other.gameObject);
        }
    }
}
