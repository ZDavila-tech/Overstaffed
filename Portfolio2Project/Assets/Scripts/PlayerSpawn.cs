using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    GameObject playerSpawn;
    GameObject player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerSpawn = GameObject.FindGameObjectWithTag("PlayerSpawn");
        player.transform.SetPositionAndRotation(playerSpawn.transform.position, playerSpawn.transform.rotation);
    }
}
