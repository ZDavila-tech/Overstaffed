using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelect : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject levelManager;
    [SerializeField] GameObject UI;
    public void SelectedFire()
    {
        PrePlayerElementSetup();
        player.GetComponent<PlayerController>().playerWeaponScript.element = NewStaff.Element.Fire;
        PostPlayerElementSetup();
    }

    public void SelectedWater()
    {
        PrePlayerElementSetup();
        player.GetComponent<PlayerController>().playerWeaponScript.element = NewStaff.Element.Water;
        PostPlayerElementSetup();
    }

    public void SelectedEarth()
    {
        PrePlayerElementSetup();
        player.GetComponent<PlayerController>().playerWeaponScript.element = NewStaff.Element.Earth;
        PostPlayerElementSetup();
    }

    public void PrePlayerElementSetup() //must happen before player element setup occurs
    {
        DestroyImmediate(Camera.main.gameObject);
        Instantiate(player);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void PostPlayerElementSetup() //must happen after player element setup occurs
    {
        Debug.Log("PlayerSpawned");
        SceneManager.LoadScene("Reception");
        Instantiate(levelManager);
        Instantiate(UI);
    }
}
