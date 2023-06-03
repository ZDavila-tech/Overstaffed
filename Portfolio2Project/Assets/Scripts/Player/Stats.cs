using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [Header("----- Stats -----")]
    [Range(10, 250)][SerializeField] int Health;
    [Range(1, 100)][SerializeField] public float Speed;
    [Range(1, 100)][SerializeField] public int Attack;
    [Range(1, 100)][SerializeField] public float wallrunSpeed;

    [Header("----- Experience -----")]
    [SerializeField] int Exp;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Getters
    public int GetHealth()
    {
        return Health;
    }
    public float GetSpeed()
    {
        return Speed;
    }
    public int GetAttack()
    {
        return Attack;
    }

    //Stat-modification methods
    public void HealthUp(int amount)
    {
        Health += amount;
    }
    public void SpeedUp (int amount)
    {
        Speed += amount;
    }
    public void AttackUp (int amount)
    {
         Attack += amount;
    }
    public void GainExp(int amount)
    {
        Exp += amount;
    }
}
