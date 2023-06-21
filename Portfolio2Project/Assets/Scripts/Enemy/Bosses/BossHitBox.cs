using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossHitBox : MonoBehaviour, IDamage
{
    [SerializeField] SusanFromHR susan;
    [SerializeField] GameObject objectToFollow;

    private void Update()
    {
        if (objectToFollow != null)
        {
            this.gameObject.transform.position = objectToFollow.transform.position;
        }
        else
        {
            if(susan.hitBoxCenter != null)
            {
                objectToFollow = susan.hitBoxCenter;
            }
        }
    }

    public void Freeze(float duration)
    {

    }

    public void Burn(float duration, float timeBetween)
    {

    }

    public void Poison(float duration, float timeBetween)
    {

    }

    public void TakeDamage(int damage)
    {
        if(susan != null)
        {
            if (susan.bossIsInvulnerable)
            {
                //play Boss Invulnerable Audio and show a message that it's not working. 
                StartCoroutine(susan.InvulnerableColorFlash());
            }
            else
            {
                susan.anim.SetTrigger("Hurt");
                switch (susan.GetBossPhase())
                {
                    case 1:
                        {
                            //play audio and animation
                            susan.healthBarOne -= damage;
                            StartCoroutine(susan.TakeDamageColorFlash());
                            break;
                        }
                    case 2:
                        {
                            //play audio and animation
                            susan.healthBarTwo -= damage;
                            StartCoroutine(susan.TakeDamageColorFlash());
                            break;
                        }
                    case 3:
                        {
                            //play audio and animation
                            susan.healthBarThree -= damage;
                            StartCoroutine(susan.TakeDamageColorFlash());
                            break;
                        }
                    case 4:
                        {
                            //needed to have something for the "else statement" in Susan's boss code.
                            //play audio or something the boss is probably dead or glitched.
                            break;
                        }
                }
            }
        }
    }
}
