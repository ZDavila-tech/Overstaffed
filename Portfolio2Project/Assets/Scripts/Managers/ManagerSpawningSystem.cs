using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerSpawningSystem : MonoBehaviour
{
    [SerializeField] gameManager gameManager;
    [SerializeField] LevelManager levelManager;
    [SerializeField] UIManager uiManager;
    [SerializeField] AudioManager audioManager;

    void Start()
    {
        if (gameManager.instance == null)
        {
            Instantiate(gameManager);
        }

        if (LevelManager.instance == null)
        {
            Instantiate(levelManager);
        }

        if (UIManager.instance == null)
        {
            Instantiate(uiManager);
        }

        if (AudioManager.instance == null)
        {
            Instantiate(audioManager);
        }

        Destroy(this.gameObject);
    }
}
