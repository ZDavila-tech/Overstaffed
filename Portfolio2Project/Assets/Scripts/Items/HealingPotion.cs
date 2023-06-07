using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPotion : MonoBehaviour
{
    [SerializeField] PlayerController controller;
    [SerializeField] public int healAmount;
    bool itemUsed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.GetComponent<MeshRenderer>().enabled != true)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            controller.TakeDamage(healAmount * -1);
            controller.items.Remove(controller.items[controller.itemSelected]);
            itemUsed = true;
            Destroy(gameObject);
        }
        
    }
}
