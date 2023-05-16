using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelect : MonoBehaviour
{
    GameObject playerSpawn;
    GameObject playerType;
    [SerializeField] GameObject playerTypeFire;
    public void SelectedFire()
    {
        SceneManager.LoadScene("Main Game");
        //playerSpawn = GameObject.FindGameObjectWithTag("PlayerSpawn");
        //playerType = Instantiate(playerTypeFire);
        //playerType.transform.SetPositionAndRotation(playerSpawn.transform.position, playerSpawn.transform.rotation);
    }

    public void SelectedWater()
    {
        //gameManager.instance.SelectedWater();
    }

    public void SelectedEarth()
    {
        //gameManager.instance.SelectedEarth();
    }
}
