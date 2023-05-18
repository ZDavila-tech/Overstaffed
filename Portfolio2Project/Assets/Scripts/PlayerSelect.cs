using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelect : MonoBehaviour
{
    [SerializeField] GameObject player;
    public void SelectedFire()
    {
        Instantiate(player);
        player.GetComponent<PlayerController>().playerWeaponScript.element = NewStaff.Element.Fire;
        Debug.Log("PlayerSpawned");
        SceneManager.LoadScene("Main Game");
    }

    public void SelectedWater()
    {
        Instantiate(player);
        player.GetComponent<PlayerController>().playerWeaponScript.element = NewStaff.Element.Water;
        Debug.Log("PlayerSpawned");
        SceneManager.LoadScene("Main Game");
    }

    public void SelectedEarth()
    {
        Instantiate(player);
        player.GetComponent<PlayerController>().playerWeaponScript.element = NewStaff.Element.Earth;
        Debug.Log("PlayerSpawned");
        SceneManager.LoadScene("Main Game");
    }
}
