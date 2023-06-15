using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakOutPrevention : MonoBehaviour
{
    [SerializeField] GameObject returnPoint;
    private GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject;
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.SetPositionAndRotation(returnPoint.transform.position, returnPoint.transform.rotation);
            player.GetComponent<CharacterController>().enabled = true;
        }
    }
}
