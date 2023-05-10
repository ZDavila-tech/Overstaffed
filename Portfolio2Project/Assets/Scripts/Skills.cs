using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Skills : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;

    [Header("----- Values ------")]
    [Header("~Dash~")]
    [Range(1,50)][SerializeField] float DashSpeed;
    [Range(0,1)][SerializeField] float DashTime;

    [Header("~High Jump~")]
    [Range(1, 50)][SerializeField] float JumpForce;
    [Range(0, 1)][SerializeField] float JumpTime;



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

    public void hiJump()
    {
        CanMove = false;
        Debug.Log("JUMP");
        GetComponent<PlayerController>().changeJumpsUsed(1);
        StartCoroutine(hiJumpCoroutine());

    }

    IEnumerator hiJumpCoroutine()
    {
        var startTime = Time.time;
        while (Time.time < startTime + JumpTime)
        {
            controller.Move(transform.up * JumpForce * Time.deltaTime);
            yield return null;
        }
        CanMove = true;

    }


    public bool canMove()
    {
        return CanMove;
    }

}
