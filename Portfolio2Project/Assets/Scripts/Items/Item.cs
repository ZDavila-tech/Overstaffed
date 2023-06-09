using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public bool isSelected;
    public bool useable;
    public bool used;
    // Start is called before the first frame update
    void Start()
    {
        isSelected = false;
        this.GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (!isSelected)
        //{
        //    this.GetComponent<MeshRenderer>().enabled = false;
        //    return;
        //}
        //this.GetComponent<MeshRenderer>().enabled = true;
        
        if (isSelected && !used)
        {
            useable = true;
            this.GetComponent<MeshRenderer>().enabled = true;
        }
        else if (!isSelected || used)
        {
            useable = false;
            this.GetComponent<MeshRenderer>().enabled = false;
        }

    }
}
