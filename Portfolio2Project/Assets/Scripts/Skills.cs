using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Skills : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;

    [Header("----- Values ------")]
    [Range(1,50)][SerializeField] float DashSpeed;
    [Range(0,1)][SerializeField] float DashTime;

    bool CanMove = true;
    public void Dash()
    {
        CanMove= false;
        Debug.Log("DASH");
        StartCoroutine(dashCoroutine());

    }

    IEnumerator dashCoroutine()
    {
        var startTime = Time.time;
        while (Time.time < startTime + DashTime)
        {
            controller.Move(transform.forward * DashSpeed * Time.deltaTime);
            yield return null;
        }
        CanMove= true;

    }


    public bool canMove()
    {
        return CanMove;
    }

}
