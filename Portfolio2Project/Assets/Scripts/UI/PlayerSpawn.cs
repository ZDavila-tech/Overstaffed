using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    GameObject playerSpawn;
    GameObject player;

    LevelManager levelManager;
    GameManager gameManager;
    bool playerInSpawn;
    bool keepPullingPlayer;

    private void Start()
    {
        levelManager = LevelManager.instance;
        gameManager = GameManager.instance;
        player = GameObject.FindGameObjectWithTag("Player");
        playerSpawn = this.gameObject;
        PullPlayer();
    }

    private void Update()
    {
        if(keepPullingPlayer == true)
        {
            PullPlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInSpawn = true;
            keepPullingPlayer = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerInSpawn = false;
        levelManager.levelStarted = true;
        Destroy(this.gameObject);
    }

    public void PullPlayer() //Player is in spawn or close enough -> Start Game
    {
        if (playerInSpawn == true)
        {
            levelManager.loadingLevel = false;
            //Debug.Log("Player spawn pulled player");
        }
        else
        {
            player.transform.SetPositionAndRotation(playerSpawn.transform.position, playerSpawn.transform.rotation);
            keepPullingPlayer = true;
            //Debug.Log("Player spawn tried to pull player player");
        }
    }
}
