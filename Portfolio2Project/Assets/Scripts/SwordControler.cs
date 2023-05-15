using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordControler : MonoBehaviour
{
    [SerializeField] public Staff staff;
    [SerializeField] private Renderer sWeapon;


    public bool attacking;
    Color color;
    // Start is called before the first frame update
    void Start()
    {
        staff.gameObject.GetComponent<Staff>();
        sWeapon.enabled = false;
        //attacking = eStaff.isAttacking;
    }

    // Update is called once per frame
    void Update()
    {
        attacking = staff.isAttacking;
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
