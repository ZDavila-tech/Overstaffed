using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPotion : MonoBehaviour
{
    [SerializeField] PlayerController controller;
    [SerializeField] public int healAmount;
    
    //bool itemUsed;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Item>().used = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (this.GetComponent<MeshRenderer>().enabled != true)
        //{
        //    this.GetComponent<Item>().useable = false;
        //    return;
        //}
        if (Input.GetKeyDown(KeyCode.Tab) && this.GetComponent<Item>().useable)
        {
            controller.TakeDamage(healAmount * -1);
            this.GetComponent<Item>().used = true;
            //controller.items.Remove(controller.items[controller.itemSelected]);
        }
        
    }
}
