using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelect : MonoBehaviour
{
    [SerializeField] GameObject player;
    public void SelectedFire()
    {
        Instantiate(player);
        SceneManager.LoadScene("Main Game");
    }

    public void SelectedWater()
    {
        Instantiate(player);
        SceneManager.LoadScene("Main Game");
    }

    public void SelectedEarth()
    {
        Instantiate(player);
        SceneManager.LoadScene("Main Game");
    }
}
