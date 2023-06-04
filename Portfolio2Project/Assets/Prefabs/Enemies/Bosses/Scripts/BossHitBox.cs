using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitBox : MonoBehaviour, IDamage
{
    [SerializeField] SusanFromHR susan;
    [SerializeField] GameObject objectToFollow;

    private void Update()
    {
        this.gameObject.transform.position = objectToFollow.transform.position;
    }

    public void TakeDamage(int damage)
    {
        if (susan.bossIsInvulnerable)
        {
            //play Boss Invulnerable Audio and show a message that it's not working. 
        }
        else
        {
            switch (susan.GetBossPhase())
            {
                case 1:
                    {
                        susan.healthBarOne -= damage;
                        break;
                    }
                case 2:
                    {
                        susan.healthBarTwo -= damage;
                        break;
                    }
                case 3:
                    {
                        susan.healthBarThree -= damage;
                        break;
                    }
                case 4:
                    {
                        //play audio or something the boss is probably dead or glitched
                        break;
                    }
            }
        }
    }
}
