using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerSpawningSystem : MonoBehaviour
{
    [SerializeField] GameObject managers;


    void Start()
    {
        if(gameManager.instance == null)
        {
            Instantiate(managers);
        }

        Destroy(this.gameObject);
    }
}
