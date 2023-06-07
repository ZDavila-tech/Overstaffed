using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour, IDamage
{
    [SerializeField] int Durability;
    [SerializeField] MeshRenderer shieldModel;
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
        Durability -= damage;
        if(Durability <= 0)
        {
            shieldModel.enabled = false;
            Destroy(gameObject);
        }
    }
}
