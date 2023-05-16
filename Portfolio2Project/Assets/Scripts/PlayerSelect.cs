using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelect : MonoBehaviour
{
    [Header("-----Player Types------")]
    [SerializeField] public Transform firePlayer;
    [SerializeField] public Transform waterPlayer;
    [SerializeField] public Transform earthPlayer;

    public void SelectedFire()
    {
        gameManager.instance.playerType = firePlayer;
        Debug.Log("playerType set to Fire Wizard");
        CharacterSelected();
        Debug.Log("Fire Wizard Selected");
    }

    public void SelectedWater()
    {
        gameManager.instance.playerType = waterPlayer;
        CharacterSelected();
        Debug.Log("Water Wizard Selected");
    }

    public void SelectedEarth()
    {
        gameManager.instance.playerType = earthPlayer;
        CharacterSelected();
        Debug.Log("Earth Wizard Selected");
    }

    public void CharacterSelected()
    {
        PlayerSpawn.CreatePlayerCharacter();
        SceneManager.LoadScene("Main Game");
        gameManager.instance.StartGame();
    }
}
