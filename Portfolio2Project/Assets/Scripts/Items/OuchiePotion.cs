using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuchiePotion : MonoBehaviour
{
    [SerializeField] PlayerController controller;
    [SerializeField] public int hurtAmount;
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
            controller.TakeDamage(hurtAmount);
            Destroy(gameObject);
        }

    }
}
