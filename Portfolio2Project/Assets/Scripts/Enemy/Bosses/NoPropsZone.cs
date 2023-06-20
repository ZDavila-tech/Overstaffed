using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoPropsZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<LaunchableProp>() != null)
        {
            other.gameObject.GetComponent<LaunchableProp>().LaunchYourself();
        }
    }
}
