using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakOutPrevention : MonoBehaviour
{
    [SerializeField] GameObject returnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.transform.SetPositionAndRotation(returnPoint.transform.position, returnPoint.transform.rotation);
        }
    }
}
