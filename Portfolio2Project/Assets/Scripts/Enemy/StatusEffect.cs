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

    public IEnumerator Freeze(float duration)
    {
        enemy.toggleMovement(false);
        freezeEffect.enabled = true;
        yield return new WaitForSeconds(duration);
        enemy.toggleMovement(true);
        freezeEffect.enabled = false;
    }
}
