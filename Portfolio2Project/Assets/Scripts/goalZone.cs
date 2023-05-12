using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goalZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) //go into zone to win/go to next level
    {
        if (gameManager.instance.enemiesRemaining <= 0 && other.CompareTag("Player"))
        {
            gameManager.instance.youWin();
        }
    }
}
