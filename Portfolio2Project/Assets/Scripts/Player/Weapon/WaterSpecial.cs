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
        gameObject.GetComponent<SphereCollider>().enabled = false; 
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        enemyRef = enemy.GetComponent<EnemyAI>();
    }

    private void OnTriggerEnter(Collider other)
    {
            if(other.tag == "Enemy")
            {
                enemyRef.isSlowed = true;
            }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            //other.GetComponent<NavMeshAgent>().speed *= slowRate;
            //Debug.Log("Enemy Lost");
            enemyRef.isSlowed = false;
        }
    }
}
