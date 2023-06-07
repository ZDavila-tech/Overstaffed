using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public bool isSelected;
    // Start is called before the first frame update
    void Start()
    {
        isSelected = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSelected)
        {
            this.GetComponent<MeshRenderer>().enabled = false;
            return;
        }
        this.GetComponent<MeshRenderer>().enabled = true;
    }
}
