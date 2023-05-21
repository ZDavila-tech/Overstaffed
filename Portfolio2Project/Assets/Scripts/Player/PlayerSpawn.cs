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
        PullPlayer();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInSpawn = true;
        }
    }

    public void PullPlayer() //Player is in spawn or close enough -> Start Game
    {
        if (playerInSpawn == true)
        {
            Debug.Log("Player spawn pulled player");
            levelManager.levelStarted = true;
            levelManager.loadingLevel = false;
            gameManager.fadeIn = false;
            Destroy(this.gameObject);
        }
        else
        {
            player.transform.SetPositionAndRotation(playerSpawn.transform.position, playerSpawn.transform.rotation);
            Debug.Log("Player spawn tried to pull player player");
        }
    }
}
