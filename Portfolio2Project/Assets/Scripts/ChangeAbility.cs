using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAbility : MonoBehaviour
{
    [SerializeField]Skills.skill skillAssign;
    private void OnTriggerEnter(Collider other)
    {
        Skills skill = other.GetComponent<Skills>();
        if (skill)
        {
            skill.setSkill(1,skillAssign);
        }
    }
}
