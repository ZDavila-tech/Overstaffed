using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{

    [SerializeField] int hitDmg;

    private void OnTriggerEnter(Collider other)
    {
            IDamage damageable = other.GetComponentInParent<IDamage>();

            if (damageable != null)
            {
                damageable.TakeDamage(hitDmg);
                DamageCooldown();
            }
    }

    IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(.5f);
    }
}
