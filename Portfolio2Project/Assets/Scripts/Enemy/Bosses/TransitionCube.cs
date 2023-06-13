using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        if (susan != null)
        {
            healthPoints -= damage;

            AudioManager.instance.CrystalShoot();

            if (healthPoints <= 0)
            {
                switch (myCubeNumber)
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
                ResetHealth();
                this.gameObject.SetActive(false);
            }
        }
    }

    public void ResetHealth()
    {
        healthPoints = originalHealth;
    }
}
