using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour, IInteract
{
    private UIManager uiManager;

    // Start is called before the first frame update
    void Start()
    {
        uiManager = UIManager.instance;
    }

    public void Interact()
    {

    }
}
