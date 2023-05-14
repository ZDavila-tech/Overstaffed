using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorControler : MonoBehaviour
{
    public Staff staff;
    [SerializeField] private Material mat;
    [SerializeField] private Renderer sword;
    public bool test;

    Color color;
    // Start is called before the first frame update
    void Start()
    {
        sword.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        test = staff.isAttacking;
        if (staff.isAttacking)
        {
            sword.enabled = true;
        }
        else
        {
            sword.enabled = false;
        }

    }
}
