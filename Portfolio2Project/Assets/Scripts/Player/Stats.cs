using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [Header("----- Stats -----")]
    [Range(0, 100)][SerializeField] public static int Health;
    [Range(0, 100)][SerializeField] public static int Speed;
    [Range(0, 100)][SerializeField] public static int Attack;
    [Range(0, 100)][SerializeField] public static float wallrunSpeed;

    [Header("----- Experience -----")]
    [SerializeField] public static int Exp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //Getters
    public int GetHealth()
    {
        return Health;
    }
    public int GetSpeed()
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
        gameManager.instance.playerController.UpdateHP(true);
        gameManager.instance.playerController.UpdateHealthBar();
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
        UIManager.instance.UpdateExp();
    }
}
