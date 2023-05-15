using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorControler : MonoBehaviour
{
    [SerializeField] public Staff staff;
    //[SerializeField] public WaterStaff wStaff;
    //[SerializeField] public EarthStaff eStaff;
    [SerializeField] private Renderer sWeapon;


    public bool attacking;
    Color color;
    // Start is called before the first frame update
    void Start()
    {
        staff.gameObject.GetComponent<Staff>();
        //wStaff.gameObject.GetComponent<WaterStaff>();
        //eStaff.gameObject.GetComponent<EarthStaff>();
        sWeapon.enabled = false;
        //attacking = eStaff.isAttacking;
    }

    // Update is called once per frame
    void Update()
    {
        //attacking = eStaff.isAttacking;
        if (staff.isAttacking)
        {
            sWeapon.enabled = true;
        }
        else
        {
            sWeapon.enabled = false;
        }

    }
}
