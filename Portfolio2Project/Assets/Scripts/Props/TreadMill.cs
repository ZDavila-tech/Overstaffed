using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreadMill : MonoBehaviour
{
    [SerializeField] float speed;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Moving");
            other.GetComponent<CharacterController>().Move(transform.forward * speed * Time.deltaTime);
        }
        if (other.CompareTag("EnemyCollision"))
        {
            Debug.Log("EnemyMoving");
            other.transform.parent.transform.Translate(transform.forward * speed * Time.deltaTime);
        }
        if (other.CompareTag("Pickup"))
        {
            Debug.Log("EnemyMoving");
            other.transform.Translate(transform.forward * speed * Time.deltaTime);
        }
    }

}
