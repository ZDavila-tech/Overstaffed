using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] int pushAmount;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3);
    }

    private void OnTriggerEnter(Collider other)
    {
        IPhysics physicsable = other.GetComponent<IPhysics>();

        if (physicsable != null)
        {
            Vector3 dir = other.transform.position - transform.position;
            physicsable.Knockback(dir * pushAmount);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
