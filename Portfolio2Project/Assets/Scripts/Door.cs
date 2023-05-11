using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject model;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            model.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            model.SetActive(true);
        }
    }
}
