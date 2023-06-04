using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionCube : MonoBehaviour, IDamage
{
    [SerializeField] SusanFromHR susan;
    [SerializeField] int healthPoints;
    [SerializeField] int myCubeNumber;
    private int originalHealth;

    private void Start()
    {
        originalHealth = healthPoints;
    }

    public void TakeDamage(int damage)
    {
        healthPoints -= damage;
        if (healthPoints <= 0 )
        {
            switch(myCubeNumber)
            {
                case 1:
                    {
                        susan.transitionCubeOneBroken = true;
                        break;
                    }
                case 2:
                    {
                        susan.transitionCubeTwoBroken = true;
                        break;
                    }
                case 3:
                    {
                        susan.transitionCubeThreeBroken = true;
                        break;
                    }
                case 4:
                    {
                        susan.transitionCubeFourBroken = true;
                        break;
                    }
            }
            this.gameObject.SetActive(false);
            ResetHealth();
        }
    }

    public void ResetHealth()
    {
        healthPoints = originalHealth;
    }
}