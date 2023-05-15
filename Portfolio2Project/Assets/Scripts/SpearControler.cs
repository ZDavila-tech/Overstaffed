using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearControler : MonoBehaviour
{
    [SerializeField] public WaterStaff wStaff;
    [SerializeField] private Renderer sWeapon;


    public bool attacking;
    Color color;
    // Start is called before the first frame update
    void Start()
    {
        wStaff.gameObject.GetComponent<WaterStaff>();
        sWeapon.enabled = false;
        attacking = wStaff.isAttacking;
    }

    // Update is called once per frame
    void Update()
    {
        attacking = wStaff.isAttacking;
        if (wStaff.isAttacking)
        {
            sWeapon.enabled = true;
        }
        else
        {
            sWeapon.enabled = false;
        }

    }
}
