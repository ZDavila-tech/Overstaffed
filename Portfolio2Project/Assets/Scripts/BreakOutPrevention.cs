using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakOutPrevention : MonoBehaviour
{
    [SerializeField] GameObject returnPoint;
    private GameObject player;
    public bool pullPlayer;

    private void Start()
    {
        pullPlayer = false;
    }

    private void Update()
    {
        if (pullPlayer && player != null)
        {
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = new Vector3(returnPoint.transform.position.x, returnPoint.transform.position.y, returnPoint.transform.position.z);
            if ((returnPoint.transform.position - player.transform.position).magnitude <= 1)
            {
                pullPlayer = false;
            }
            player.GetComponent<CharacterController>().enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject;
            pullPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject;
            pullPlayer = true;
        }
    }
}
