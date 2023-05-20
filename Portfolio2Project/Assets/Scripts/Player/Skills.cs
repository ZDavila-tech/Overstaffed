using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class Skills : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] PlayerController playerController;
    [SerializeField] Transform blinkAimIndicatorPrefab;
    public enum skill
    {
        Dash, HiJump, SlowFall, Blink, Invisibility
    }

    [Header("----- Values ------")]
    [Header("~Dash~")]
    [Range(1, 50)][SerializeField] float DashSpeed;
    [Range(0, 1)][SerializeField] float DashTime;
    [Range(1, 20)][SerializeField] float DashCooldown;
    bool canDash = true;

    [Header("~High Jump~")]
    [Range(1, 50)][SerializeField] float JumpForce;
    [Range(0, 1)][SerializeField] float JumpTime;
    [Range(1, 20)][SerializeField] float HiJumpCooldown;
    bool canHiJump = true;

    [Header("~Slow Fall~")]
    [Range(1, 50)][SerializeField] float NewGravityForce;
    [Range(1, 20)][SerializeField] float SlowFallCooldown;
    bool canSlowFall = true;

    [Header("~Blink~")]
    [Range(1, 50)][SerializeField] float BlinkDistance;
    [Range(1, 20)][SerializeField] float BlinkCooldown;
    bool canBlink = true;
    bool aiming;

    skill activeSkill1 = skill.HiJump;
    skill activeSkill2 = skill.Dash;
    skill activeSkill3 = skill.Blink;


    bool CanMove = true;
    float gravityOrig;
    Transform blinkAimIndicator;
    public void Dash()
    {
        if (canDash)
        {
            canDash = false;
            CanMove = false;
            playerController.ChangeJumpsUsed(1);
            StartCoroutine(dashCoroutine());
            StartCoroutine(dashCooldownCoroutine());
        }


    }

    IEnumerator dashCoroutine()
    {
        var startTime = Time.time;
        while (Time.time < startTime + DashTime)
        {
            controller.Move(transform.forward * DashSpeed * Time.deltaTime);
            yield return null;
        }
        CanMove = true;
        StopCoroutine(dashCoroutine());

    }

    IEnumerator dashCooldownCoroutine()
    {
        yield return new WaitForSeconds(DashCooldown);
        canDash = true;
    }

    public bool isDashCooldown()
    {
        if(!canDash)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void hiJump()
    {
        if (canHiJump)
        {
            canHiJump = false;
            CanMove = false;
            playerController.ChangeJumpsUsed(1);
            StartCoroutine(hiJumpCoroutine());
            StartCoroutine(hiJumpCooldownCoroutine());
        }

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
        StopCoroutine(hiJumpCoroutine());

    }

    IEnumerator hiJumpCooldownCoroutine()
    {
        yield return new WaitForSeconds(HiJumpCooldown);
        canHiJump = true;
    }

    public bool isJumpCooldown()
    {
        if (!canHiJump)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void slowFall()
    {
        if (canSlowFall)
        {
            canSlowFall = false;
            StartCoroutine(slowFallCoroutine());
            StartCoroutine(slowFallCooldownCoroutine());
        }
    }

    IEnumerator slowFallCoroutine()
    {
        gravityOrig = playerController.ChangeGravity(NewGravityForce);
        while (!controller.isGrounded)
        {
            if (controller.velocity.y > 0)
            {
                playerController.ChangeGravity(gravityOrig);
            }
            else
            {
                playerController.ChangeGravity(NewGravityForce);
            }
            yield return null;
        }
        playerController.ChangeGravity(gravityOrig);
        StopCoroutine(slowFallCoroutine());

    }

    IEnumerator slowFallCooldownCoroutine()
    {
        yield return new WaitForSeconds(SlowFallCooldown);
        canSlowFall = true;
    }

    public void blinkAim()
    {
        if (canBlink)
        {
            canBlink = false;
            aiming = true;
            StartCoroutine(blinkAimCoroutine());
            StartCoroutine(blinkCooldownCoroutine());
        }
    }

    IEnumerator blinkAimCoroutine()
    {

        while (aiming)
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, BlinkDistance))
            {
                if (!blinkAimIndicator)
                {
                    blinkAimIndicator = Instantiate(blinkAimIndicatorPrefab);
                }
                blinkAimIndicator.position = hit.point;
            }
            else
            {
                if (blinkAimIndicator)
                {
                    Destroy(blinkAimIndicator.gameObject);
                }

            }
                if (Input.GetAxis("Movement3") == 0)
                {
                    blinkFire();
                }

            yield return null;
        }

    }
    public bool isBlinkCooldown()
    {
        if (!canBlink)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    IEnumerator blinkCooldownCoroutine()
    {
        yield return new WaitForSeconds(BlinkCooldown);
        canBlink = true;
    }

    public void blinkFire()
    {
        controller.enabled = false;
        aiming = false;
        StopCoroutine(blinkAimCoroutine());
        if (blinkAimIndicator)
        {
            transform.position = new Vector3(blinkAimIndicator.position.x, blinkAimIndicator.position.y + 1, blinkAimIndicator.position.z);

            Destroy(blinkAimIndicator.gameObject);
        }
        controller.enabled = true;

    }


    public bool canMove()
    {
        return CanMove;
    }

    public void setSkill(int slot, skill skill)
    {
        if (slot == 1)
        {
            activeSkill1 = skill;
        }
        else if (slot == 2)
        {
            activeSkill2 = skill;
        }
        else if(slot == 3)
        {
            activeSkill3 = skill;
        }
    }

    public void useSkill(int slot)
    {
        if (slot == 1)
        {
            Action skill = testSkill(activeSkill1);
            skill();
        }
        else if (slot == 2)
        {
            Action skill = testSkill(activeSkill2);
            skill();
        }
        else if (slot == 3)
        {
            Action skill = testSkill(activeSkill3);
            skill();
        }
    }

    public float getCooldown(skill sk)
    {
        switch (sk)
        {
            case (skill.Dash):
                {
                    return DashCooldown;
                }
            case (skill.HiJump):
                {
                    return HiJumpCooldown;
                }
            case (skill.SlowFall):
                {
                    return SlowFallCooldown;
                }
            case (skill.Blink):
                {
                    return BlinkCooldown;
                }
        }
        return 0;
    }
    Action testSkill(skill sk)
    {
        switch (sk)
        {
            case (skill.Dash):
                {
                    return Dash;
                }
            case (skill.HiJump):
                {
                    return hiJump;
                }
            case (skill.SlowFall):
                {
                    return slowFall;
                }
            case (skill.Blink):
                {
                    return blinkAim;
                }
        }
        return null;


    }


}
