using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaterSpecial : MonoBehaviour
{
    GameObject enemy;
    EnemyAI enemyRef;
    [SerializeField] public float slowRate;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        enemy = GameObject.FindGameObjectWithTag("Enemy");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<NavMeshAgent>().speed /= slowRate;
            Debug.Log("Enemy Detected");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<NavMeshAgent>().speed *= slowRate;
            Debug.Log("Enemy Lost");
        }
    }
}
