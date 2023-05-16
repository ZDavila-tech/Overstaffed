using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerControler : MonoBehaviour
{
    [SerializeField] public EarthStaff eStaff;
    [SerializeField] private Renderer sWeapon;


    public bool attacking;
    Color color;
    // Start is called before the first frame update
    void Start()
    {
        eStaff.gameObject.GetComponent<EarthStaff>();
        sWeapon.enabled = false;
        attacking = eStaff.isAttacking;
    }

    // Update is called once per frame
    void Update()
    {
        attacking = eStaff.isAttacking;
        if (eStaff.isAttacking)
        {
            sWeapon.enabled = true;
        }
        else
        {
            sWeapon.enabled = false;
        }

    }
}
