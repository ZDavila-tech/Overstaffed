using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    static GameObject playerSpawn;
    static Transform playerCharacter;

    public static void CreatePlayerCharacter()
    {
        Debug.Log("Player Spawn Create Player Loaded");
        playerSpawn = GameObject.FindGameObjectWithTag("PlayerSpawn");
        Instantiate(gameManager.instance.playerType);
        playerCharacter.transform.SetLocalPositionAndRotation(playerSpawn.transform.position, playerSpawn.transform.rotation);
        Debug.Log("Player Spawn Create Player Fired");
    }
}
