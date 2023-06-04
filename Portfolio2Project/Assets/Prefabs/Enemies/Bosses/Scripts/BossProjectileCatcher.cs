using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectileCatcher : MonoBehaviour
{
    public GameObject expectedEnemy;

    private void OnTriggerEnter(Collider other)
    {
        if (expectedEnemy != null)
        {
            if (other.gameObject == expectedEnemy)
            {
                //Do an explosion or something
                Destroy(expectedEnemy);
            }
        }
    }
}
