using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Skills : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] PlayerController playerController;

    [Header("----- Values ------")]
    [Header("~Dash~")]
    [Range(1,50)][SerializeField] float DashSpeed;
    [Range(0,1)][SerializeField] float DashTime;

    [Header("~High Jump~")]
    [Range(1, 50)][SerializeField] float JumpForce;
    [Range(0, 1)][SerializeField] float JumpTime;

    [Header("~Slow Fall~")]
    [Range(1, 50)][SerializeField] float NewGravityForce;



    bool CanMove = true;
    float gravityOrig;
    public void Dash()
    {
        CanMove= false;
        Debug.Log("DASH");
        playerController.changeJumpsUsed(1);
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
        playerController.changeJumpsUsed(1);
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

    public void slowFall()
    {

        Debug.Log("SlOW");
        StartCoroutine(slowFallCoroutine());

    }

    IEnumerator slowFallCoroutine()
    {
        gravityOrig = playerController.changeGravity(NewGravityForce);
        while(!controller.isGrounded)
        {
            if (controller.velocity.y > 0)
            {
                playerController.changeGravity(gravityOrig);
            }
            else
            {
                playerController.changeGravity(NewGravityForce);
            }
            yield return null;
        }
        playerController.changeGravity(gravityOrig);

    }


    public bool canMove()
    {
        return CanMove;
    }

}
