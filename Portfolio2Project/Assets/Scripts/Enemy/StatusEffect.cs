using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StatusEffect : MonoBehaviour
{
    [SerializeField] MeshRenderer freezeEffect;
    [SerializeField] EnemyAI enemy;
    [SerializeField] List<SkinnedMeshRenderer> skinnedModels;
    // Start is called before the first frame update
    void Start()
    {

    }

    IEnumerator Freezing(float duration)
    {
        enemy.toggleMovement(false);
        freezeEffect.enabled = true;
        yield return new WaitForSeconds(duration);
        enemy.toggleMovement(true);
        freezeEffect.enabled = false;
    }

    public void Freeze (float duration)
    {
        StartCoroutine(Freezing(duration));
    }

    IEnumerator Burning(float duration, float timeBetween, int damage)
    {
        bool burning = true;
        while(burning)
        {
            yield return new WaitForSeconds(timeBetween);
            enemy.TakeDamage(damage);
        }
        yield return new WaitForSeconds(duration);
        burning = false;
    }
}
