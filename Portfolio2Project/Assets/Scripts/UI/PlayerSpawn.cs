using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    GameObject player;

    LevelManager levelManager;

    bool playerInSpawn;
    bool keepPullingPlayer;

    private void Start()
    {
        levelManager = LevelManager.instance;
        player = GameObject.FindGameObjectWithTag("Player");
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
            levelManager.inElevator = false;
            keepPullingPlayer = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerLeftSpawn();
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
            if(player != null)
            {
                player.GetComponent<CharacterController>().enabled = false;
                player.transform.SetPositionAndRotation(this.gameObject.transform.position, this.gameObject.transform.rotation);
                player.GetComponent<CharacterController>().enabled = true;
            }
            else
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }

            keepPullingPlayer = true;
            //Debug.Log("Player spawn tried to pull player player");
        }
    }

    public void PlayerLeftSpawn()
    {
        playerInSpawn = false;
        levelManager.levelStarted = true;
        levelManager.levelCompleted = false;
        Destroy(this.gameObject);
    }
}
